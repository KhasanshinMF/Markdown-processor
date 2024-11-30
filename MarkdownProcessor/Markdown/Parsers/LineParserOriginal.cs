using System.Collections;
using Markdown.Enums;
using Markdown.TagTree;

namespace Markdown.Classes;

public class LineParserOriginal
{
    private Stack<(int, Tag)> _tagStack;
    private Stack<(int, Tag)> _textStack;
    private bool _inHeading;
    private bool _inBold;
    private bool _inItalic;
    private List<Tag> _parsedMarkdownLine;
    private TagNode _tagTree;

    public LineParserOriginal()
    {
        _tagStack = new Stack<(int, Tag)>();
        _textStack = new Stack<(int, Tag)>();
        _inHeading = false;
        _inBold = false;
        _inItalic = false;
        _parsedMarkdownLine = new List<Tag>();
        _tagTree = new TagNode(new Tag(TagType.Root));
    }
    
    public List<Tag> ParseLine(string markdownLine)
    {
        int i = 0;
        while (i < markdownLine.Length)
        {
            if (IsEscaped(markdownLine, i))
            {
                if (_textStack.Count == 0)
                    _parsedMarkdownLine.Add(ParsingEscapedText(markdownLine, ref i));
                else _textStack.Push((i, ParsingEscapedText(markdownLine, ref i)));
            }
            
            else if (IsHeader(markdownLine, i))
            {
                _parsedMarkdownLine.Add(HeaderParsing(markdownLine, ref i));
                _inHeading = true;
            }
            
            else if (IsBold(markdownLine, i))
            {
                if (!_inItalic) BoldParsing(ref i);
                i += 2;
            }
            
            else if (IsItalic(markdownLine, i))
            {
                ItalicParsing(ref i);
                i++;
            }

            // Парсим обычный текст
            else if (_tagStack.Count != 0) _textStack.Push((i, TextParsing(markdownLine, ref i)));
            else _parsedMarkdownLine.Add(TextParsing(markdownLine, ref i));
        }
        if (_inHeading) _parsedMarkdownLine.Add(new Tag(Enums.TagType.HeaderClose, _parsedMarkdownLine[0].HeaderLevel));
        return _parsedMarkdownLine;
    }

    private bool IsEscaped(string markdownLine, int i)
    {
        return markdownLine[i] == '\\' && i + 1 < markdownLine.Length &&
               (markdownLine[i + 1] == '#' || markdownLine[i + 1] == '_' || markdownLine[i + 1] == '\\');
    }

    private bool IsHeader(string markdownLine, int i)
    {
        return markdownLine[i] == '#' && (i == 0 || markdownLine[i - 1] == '\n' || markdownLine[i - 1] == '\r');
    }

    private bool IsBold(string markdownLine, int i)
    {
        return i + 1 < markdownLine.Length && markdownLine[i] == '_' && markdownLine[i + 1] == '_';
    }

    private bool IsItalic(string markdownLine, int i)
    {
        return markdownLine[i] == '_' && !(i + 1 < markdownLine.Length && markdownLine[i + 1] == '_');
    }

    private Tag HeaderParsing(string markdownText, ref int i)
    {
        // Считаем уровень заголовка
        var headerLevel = 0;
        while (i < markdownText.Length && markdownText[i] == '#' && headerLevel < 6)
        {
            headerLevel++;
            i++;
        }
        // Пропускаем пробелы между тэгом и текстом
        while (i < markdownText.Length && markdownText[i] == ' ') i++;
        
        return new Tag(Enums.TagType.HeaderOpen, headerLevel);
    }

    private void BoldParsing(ref int i)
    {
        if (!_inBold)
        {
            _inBold = true;
            _tagStack.Push((i, new Tag(Enums.TagType.BoldOpen)));
        }
        else
        {
            _inBold = false;
            var (openingTagIndex, openingTag) = _tagStack.Pop();
            _parsedMarkdownLine.Add(openingTag);
            
            var (textTagIndex, text) = _textStack.Peek();
            while (openingTagIndex < textTagIndex)
            {
                _parsedMarkdownLine.Add(text);
                _textStack.Pop();
                if (_textStack.Count == 0) break;
                (textTagIndex, text) = _textStack.Peek();
            }
            
            _parsedMarkdownLine.Add(new Tag(Enums.TagType.BoldClose));
        }
    }
    
    private void ItalicParsing(ref int i)
    {
        if (!_inItalic)
        {
            _inItalic = true;
            _tagStack.Push((i, new Tag(Enums.TagType.ItalicOpen)));
        }
        else
        {
            _inItalic = false;
            var (openingTagIndex, openingTag) = _tagStack.Pop();
            _parsedMarkdownLine.Add(openingTag);
            
            var (textTagIndex, text) = _textStack.Peek();
            while (openingTagIndex < textTagIndex)
            {
                _parsedMarkdownLine.Add(text);
                _textStack.Pop();
                if (_textStack.Count == 0) break;
                (textTagIndex, text) = _textStack.Peek();
            }
            _parsedMarkdownLine.Add(new Tag(Enums.TagType.ItalicClose));
        }
    }

    private Tag TextParsing(string markdownText, ref int i)
    {
        var startIndex = i;
        
        if (_inItalic)
        {
            while (i < markdownText.Length && !IsEscaped(markdownText, i))
            {
                if (IsItalic(markdownText, i)) break;
                i++;
            }
        }
        else
        {
            while (i < markdownText.Length && markdownText[i] != '_' && !IsEscaped(markdownText, i)) i++;
        }
        return new Tag(Enums.TagType.Text, markdownText.Substring(startIndex, i - startIndex));
    }

    private Tag ParsingEscapedText(string markdownText, ref int i)
    {
        i++;
        var startIndex = i;
    
        if (markdownText[i] == '\\' || IsItalic(markdownText, i)) i++;
        else if (IsBold(markdownText, i)) i += 2;
        
        while (i < markdownText.Length &&
               markdownText[i] != '_' &&
               markdownText[i] != '\\') i++;
    
        return new Tag(Enums.TagType.Text, markdownText.Substring(startIndex, i - startIndex));
    }
}