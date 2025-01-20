namespace Markdown.MarkdownProcessor;

public class Document
{
    public List<Line> Lines { get; }

    public Document(List<Line> lines)
    {
        Lines = lines;
    } 
}