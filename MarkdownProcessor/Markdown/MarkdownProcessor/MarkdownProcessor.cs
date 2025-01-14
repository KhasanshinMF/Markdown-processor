using Markdown.Parsers;
using Markdown.Renderers;

namespace Markdown.MarkdownProcessor
{
    public class MarkdownProcessor : IMarkdownProcessor
    {
        public string ConvertToHtml(string markdownText)
        {
            Parser parser = new Parser();
            Renderer renderer = new Renderer();
            var parsedMarkdown = parser.Parse(markdownText);
            return renderer.ToHtml(parsedMarkdown);
        }
    }
}