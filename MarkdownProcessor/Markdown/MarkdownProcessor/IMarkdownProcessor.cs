namespace Markdown.MarkdownProcessor
{
    public interface IMarkdownProcessor
    {
        string ConvertToHtml(string markdownText);
    }
}