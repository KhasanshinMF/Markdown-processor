using Markdown.MarkdownProcessor;
using Markdown.Tags;

namespace Markdown.Parsers;

public class LineParser
{
    private readonly Stack<Tag> _tagStack;
    private readonly Stack<Tag> _textStack;
    private bool _inHeading;
    private bool _inBold;
    private bool _inItalic;
    private readonly List<Tag> _parsedLine;

    public LineParser()
    {
        _tagStack = new Stack<Tag>();
        _textStack = new Stack<Tag>();
        _inHeading = false;
        _inBold = false;
        _inItalic = false;
        _parsedLine = new List<Tag>();
    }

    public Line ParseLine(string line)
    {
        int i = 0;
        while (i < line.Length)
        {
            if (IsEscaped(line, i))
                i = ParseEscaping(line, i);

            else if (IsHeader(line, i))
                i = ParseHeaders(line, i);

            else if (IsBold(line, i))
                i = ParseBold(line, i);

            else if (IsItalic(line, i))
                i = ParseItalic(line, i);

            else i = ParseText(line, i);
        }

        if (_inHeading)
            _parsedLine.Add(new Tag(TagType.HeaderClose, _parsedLine[0].HeaderLevel));

        if (_inItalic || _inBold)
            AddUnclosedTags();

        return new Line(_parsedLine);
    }

    private bool IsEscaped(string line, int i)
    {
        return line[i] == '\\' && i + 1 < line.Length &&
               (line[i + 1] == '#' || line[i + 1] == '_' || line[i + 1] == '\\');
    }

    private bool IsHeader(string line, int i)
    {
        return line[i] == '#' && (i == 0 || line[i - 1] == '\n' || line[i - 1] == '\r');
    }

    private bool IsBold(string line, int i)
    {
        return i + 1 < line.Length && line[i] == '_' && line[i + 1] == '_';
    }

    private bool IsItalic(string line, int i)
    {
        return line[i] == '_' && !(i + 1 < line.Length && line[i + 1] == '_');
    }

    private int ParseEscaping(string line, int i)
    {
        i++;
        var startIndex = i;

        if (line[i] == '\\' || IsItalic(line, i)) i++;
        else if (IsBold(line, i)) i += 2;

        while (i < line.Length &&
               line[i] != '_' &&
               line[i] != '\\') i++;

        var newTag = new Tag(startIndex - 1, TagType.Text,
            line.Substring(startIndex, i - startIndex));

        if (_textStack.Count == 0)
            _parsedLine.Add(newTag);
        else
            _textStack.Push(newTag);
        return i;
    }

    private int ParseHeaders(string line, int i)
    {
        // Считаем уровень заголовка
        var headerLevel = 0;
        while (i < line.Length && line[i] == '#' && headerLevel < 6)
        {
            headerLevel++;
            i++;
        }

        // Пропускаем пробелы между тэгом и текстом
        while (i < line.Length && line[i] == ' ') i++;

        _parsedLine.Add(new Tag(TagType.HeaderOpen, headerLevel));
        _inHeading = true;
        return i;
    }

    private int ParseBold(string line, int i)
    {
        if (!_inItalic && !_inBold)
        {
            if (ClosingTagExists(line, TagType.BoldOpen, i))
            {
                _inBold = true;
                _tagStack.Push(new Tag(i, TagType.BoldOpen));
            }
            else _parsedLine.Add(new Tag(i, TagType.Text, "__"));
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

    private int ParseItalic(string line,int i)
    {
        if (!_inItalic)
        {
            if (ClosingTagExists(line, TagType.ItalicOpen, i))
            {
                _inItalic = true;
                _tagStack.Push(new Tag(i, TagType.ItalicOpen));
            }
            else _parsedLine.Add(new Tag(i, TagType.Text, "_"));
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
        var textTag = _textStack.Peek();

        while (openingTag.Index < textTag.Index)
        {
            tempParsedLine.Add(textTag);
            _textStack.Pop();
            if (_textStack.Count == 0) break;
            textTag = _textStack.Peek();
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
        _parsedLine.AddRange(tempParsedLine);
        _parsedLine.Add(new Tag(i, tagType));
    }

    private int ParseText(string line, int i)
    {
        var startIndex = i;
        while (i < line.Length && line[i] != '_' && !IsEscaped(line, i)) i++;

        var newTag = new Tag(startIndex, TagType.Text, line.Substring(startIndex, i - startIndex));

        if (_tagStack.Count != 0)
            _textStack.Push(newTag);
        else
            _parsedLine.Add(newTag);

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

        _parsedLine.AddRange(tempParsedLine);
    }

    private bool ClosingTagExists(string line, TagType tagType, int i)
    {
        if (tagType == TagType.ItalicOpen)
        {
            i++;
            while (i < line.Length)
            {
                if (IsBold(line, i)) i += 2;
                else if (IsItalic(line, i)) return true;
                i++;
            }
        }
        else if (tagType == TagType.BoldOpen)
        {
            i += 2;
            while (i < line.Length)
            {
                if (IsBold(line, i)) return true;
                i++;
            }
        }

        return false;
    }
}