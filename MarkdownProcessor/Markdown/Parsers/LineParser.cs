using Markdown.Tags;

namespace Markdown.Parsers;

public class LineParser
{
    private readonly Stack<Tag> _tagStack;
    private readonly Stack<Tag> _textStack;
    private bool _inHeading;
    private bool _inBold;
    private bool _inItalic;
    private readonly List<Tag> _parsedMarkdownLine;
    private string _markdownLine;

    public LineParser(string markdownLine)
    {
        _tagStack = new Stack<Tag>();
        _textStack = new Stack<Tag>();
        _inHeading = false;
        _inBold = false;
        _inItalic = false;
        _parsedMarkdownLine = new List<Tag>();
        _markdownLine = markdownLine;
    }

    public List<Tag> ParseLine()
    {
        int i = 0;
        while (i < _markdownLine.Length)
        {
            if (IsEscaped(i))
                i = ParseEscaping(i);

            else if (IsHeader(i))
                i = ParseHeaders(i);

            else if (IsBold(i))
                i = ParseBold(i);

            else if (IsItalic(i))
                i = ParseItalic(i);

            else i = ParseText(i);
        }

        if (_inHeading)
            _parsedMarkdownLine.Add(new Tag(TagType.HeaderClose, _parsedMarkdownLine[0].HeaderLevel));

        if (_inItalic || _inBold)
            AddUnclosedTags();

        return _parsedMarkdownLine;
    }

    private bool IsEscaped(int i)
    {
        return _markdownLine[i] == '\\' && i + 1 < _markdownLine.Length &&
               (_markdownLine[i + 1] == '#' || _markdownLine[i + 1] == '_' || _markdownLine[i + 1] == '\\');
    }

    private bool IsHeader(int i)
    {
        return _markdownLine[i] == '#' && (i == 0 || _markdownLine[i - 1] == '\n' || _markdownLine[i - 1] == '\r');
    }

    private bool IsBold(int i)
    {
        return i + 1 < _markdownLine.Length && _markdownLine[i] == '_' && _markdownLine[i + 1] == '_';
    }

    private bool IsItalic(int i)
    {
        return _markdownLine[i] == '_' && !(i + 1 < _markdownLine.Length && _markdownLine[i + 1] == '_');
    }

    private int ParseEscaping(int i)
    {
        i++;
        var startIndex = i;

        if (_markdownLine[i] == '\\' || IsItalic(i)) i++;
        else if (IsBold(i)) i += 2;

        while (i < _markdownLine.Length &&
               _markdownLine[i] != '_' &&
               _markdownLine[i] != '\\') i++;

        var newTag = new Tag(startIndex - 1, TagType.Text,
            _markdownLine.Substring(startIndex, i - startIndex));

        if (_textStack.Count == 0)
            _parsedMarkdownLine.Add(newTag);
        else
            _textStack.Push(newTag);
        return i;
    }

    private int ParseHeaders(int i)
    {
        // Считаем уровень заголовка
        var headerLevel = 0;
        while (i < _markdownLine.Length && _markdownLine[i] == '#' && headerLevel < 6)
        {
            headerLevel++;
            i++;
        }

        // Пропускаем пробелы между тэгом и текстом
        while (i < _markdownLine.Length && _markdownLine[i] == ' ') i++;

        _parsedMarkdownLine.Add(new Tag(TagType.HeaderOpen, headerLevel));
        _inHeading = true;
        return i;
    }

    private int ParseBold(int i)
    {
        if (!_inItalic && !_inBold)
        {
            if (ClosingTagExists(TagType.BoldOpen, i))
            {
                _inBold = true;
                _tagStack.Push(new Tag(i, TagType.BoldOpen));
            }
            else _parsedMarkdownLine.Add(new Tag(i, TagType.Text, "__"));
        }
        else if (!_inItalic && _inBold && _textStack.Count > 0)
        {
            _inBold = false;
            AddTextNestedInTags(TagType.BoldClose, i);
        }
        else _textStack.Push(new Tag(i, TagType.Text, "__"));
        
        i += 2;
        
        return i;
    }

    private int ParseItalic(int i)
    {
        if (!_inItalic)
        {
            if (ClosingTagExists(TagType.ItalicOpen, i))
            {
                _inItalic = true;
                _tagStack.Push(new Tag(i, TagType.ItalicOpen));
            }
            else _parsedMarkdownLine.Add(new Tag(i, TagType.Text, "_"));
        }
        else if (_inItalic)
        {
            _inItalic = false;
            AddTextNestedInTags(TagType.ItalicClose, i);
        }
        else _textStack.Push(new Tag(i, TagType.Text, "_"));

        i++;
        return i;
    }

    private void AddTextNestedInTags(TagType tagType, int i)
    {
        var tempParsedLine = new List<Tag>();
        var openingTag = _tagStack.Pop();
        var text = _textStack.Peek();

        while (openingTag.Index < text.Index)
        {
            tempParsedLine.Add(text);
            _textStack.Pop();
            if (_textStack.Count == 0) break;
            text = _textStack.Peek();
        }

        if (tagType == TagType.ItalicClose && _inBold)
        {
            _textStack.Push(openingTag);
            tempParsedLine.Add(new Tag(i, tagType));
            tempParsedLine.ForEach(_textStack.Push);
            return;
        }

        tempParsedLine.Add(openingTag);
        tempParsedLine.Reverse();
        _parsedMarkdownLine.AddRange(tempParsedLine);
        _parsedMarkdownLine.Add(new Tag(i, tagType));
    }

    private int ParseText(int i)
    {
        var startIndex = i;
        while (i < _markdownLine.Length && _markdownLine[i] != '_' && !IsEscaped(i)) i++;

        var newTag = new Tag(startIndex, TagType.Text, _markdownLine.Substring(startIndex, i - startIndex));

        if (_tagStack.Count != 0)
            _textStack.Push(newTag);
        else
            _parsedMarkdownLine.Add(newTag);

        return i;
    }

    private void AddUnclosedTags()
    {
        var tempParsedLine = new List<Tag>();
        while (_tagStack.Count() != 0 && _textStack.Count() != 0)
        {
            var text = _textStack.Pop();
            var unclosedTag = _tagStack.Pop();

            unclosedTag = new Tag(unclosedTag.Index, TagType.Text, unclosedTag.Type == TagType.BoldOpen ? "__" : "_");

            tempParsedLine.Insert(0, text);
            tempParsedLine.Insert(0, unclosedTag);
        }

        _parsedMarkdownLine.AddRange(tempParsedLine);
    }

    private bool ClosingTagExists(TagType tagType, int i)
    {
        if (tagType == TagType.ItalicOpen)
        {
            i++;
            while (i < _markdownLine.Length)
            {
                if (IsBold(i)) i += 2;
                else if (IsItalic(i)) return true;
                i++;
            }
        }
        else if (tagType == TagType.BoldOpen)
        {
            i += 2;
            while (i < _markdownLine.Length)
            {
                if (IsBold(i)) return true;
                i++;
            }
        }

        return false;
    }
}