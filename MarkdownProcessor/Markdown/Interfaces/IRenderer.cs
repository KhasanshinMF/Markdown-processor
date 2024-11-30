using Markdown.Classes;

namespace Markdown.Interfaces
{
    internal interface IRenderer
    {
        string HtmlRender(List<List<Tag>> parsedMarkdownText);
    }
}
