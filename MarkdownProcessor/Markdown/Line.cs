using Markdown.Tags;

namespace Markdown.MarkdownProcessor;

public class Line
{
    public List<Tag> Tags { get; }

    public Line(List<Tag> tags)
    {
        Tags = tags;
    }
}