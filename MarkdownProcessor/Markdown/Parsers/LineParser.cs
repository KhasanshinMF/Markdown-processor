using Markdown.MarkdownProcessor;
using Markdown.Tags;

namespace Markdown.Parsers;

public class LineParser
{
    public Line ParseLine(string line)
    {
        var state = new ParsingState();
        
        int i = 0;
        while (i < line.Length)
        {
            if (IsEscaped(line, i))
                i = ParseEscaping(line, i, state);

            else if (IsHeader(line, i))
                i = ParseHeaders(line, i, state);

            else if (IsBold(line, i))
                i = ParseBold(line, i, state);

            else if (IsItalic(line, i))
                i = ParseItalic(line, i, state);

            else i = ParseText(line, i, state);
        }

        if (state.InHeading)
            state.ParsedTags.Add(new Tag(TagType.HeaderClose, state.ParsedTags[0].HeaderLevel));

        if (state.InItalic || state.InBold)
            AddUnclosedTags(state);

        return new Line(state.ParsedTags);
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

    private int ParseEscaping(string line, int i, ParsingState state)
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

        if (state.TextStack.Count == 0)
            state.ParsedTags.Add(newTag);
        else
            state.TextStack.Push(newTag);
        return i;
    }

    private int ParseHeaders(string line, int i, ParsingState state)
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

        state.ParsedTags.Add(new Tag(TagType.HeaderOpen, headerLevel));
        state.InHeading = true;
        return i;
    }

    private int ParseBold(string line, int i, ParsingState state)
    {
        if (!state.InItalic && !state.InBold)
        {
            if (ClosingTagExists(line, TagType.BoldOpen, i))
            {
                state.InBold = true;
                state.TagStack.Push(new Tag(i, TagType.BoldOpen));
            }
            else state.ParsedTags.Add(new Tag(i, TagType.Text, "__"));
        }
        else if (!state.InItalic && state.InBold && state.TextStack.Count > 0)
        {
            state.InBold = false;
            AddTextNestedInTags(TagType.BoldClose, i, state);
        }
        else state.TextStack.Push(new Tag(i, TagType.Text, "__"));
        
        i += 2;
        
        return i;
    }

    private int ParseItalic(string line,int i, ParsingState state)
    {
        if (!state.InItalic)
        {
            if (ClosingTagExists(line, TagType.ItalicOpen, i))
            {
                state.InItalic = true;
                state.TagStack.Push(new Tag(i, TagType.ItalicOpen));
            }
            else state.ParsedTags.Add(new Tag(i, TagType.Text, "_"));
        }
        else if (state.InItalic)
        {
            state.InItalic = false;
            AddTextNestedInTags(TagType.ItalicClose, i, state);
        }
        else state.TextStack.Push(new Tag(i, TagType.Text, "_"));

        i++;
        return i;
    }

    private void AddTextNestedInTags(TagType tagType, int i, ParsingState state)
    {
        var tempParsedLine = new List<Tag>();
        var openingTag = state.TagStack.Pop();
        var textTag = state.TextStack.Peek();

        while (openingTag.Index < textTag.Index)
        {
            tempParsedLine.Add(textTag);
            state.TextStack.Pop();
            if (state.TextStack.Count == 0) break;
            textTag = state.TextStack.Peek();
        }

        if (tagType == TagType.ItalicClose && state.InBold)
        {
            state.TextStack.Push(openingTag);
            tempParsedLine.Add(new Tag(i, tagType));
            tempParsedLine.ForEach(state.TextStack.Push);
            return;
        }

        tempParsedLine.Add(openingTag);
        tempParsedLine.Reverse();
        state.ParsedTags.AddRange(tempParsedLine);
        state.ParsedTags.Add(new Tag(i, tagType));
    }

    private int ParseText(string line, int i, ParsingState state)
    {
        var startIndex = i;
        while (i < line.Length && line[i] != '_' && !IsEscaped(line, i)) i++;

        var newTag = new Tag(startIndex, TagType.Text, line.Substring(startIndex, i - startIndex));

        if (state.TagStack.Count != 0)
            state.TextStack.Push(newTag);
        else
            state.ParsedTags.Add(newTag);

        return i;
    }

    private void AddUnclosedTags(ParsingState state)
    {
        var tempParsedLine = new List<Tag>();
        while (state.TagStack.Count() != 0 && state.TextStack.Count() != 0)
        {
            var text = state.TextStack.Pop();
            var unclosedTag = state.TagStack.Pop();

            unclosedTag = new Tag(unclosedTag.Index, TagType.Text, unclosedTag.Type == TagType.BoldOpen ? "__" : "_");

            tempParsedLine.Insert(0, text);
            tempParsedLine.Insert(0, unclosedTag);
        }

        state.ParsedTags.AddRange(tempParsedLine);
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