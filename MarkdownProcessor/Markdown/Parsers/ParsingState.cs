using Markdown.Tags;

namespace Markdown.Parsers;

public class ParsingState
{
    public readonly Stack<Tag> TagStack = new();
    public readonly Stack<Tag> TextStack = new();
    public readonly List<Tag> ParsedTags = new();
    public bool InHeading { get; set; }
    public bool InBold { get; set; }
    public bool InItalic { get; set; }
}