using Markdown.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes
{
    public class MarkdownProcessor: IMarkdownProcessor
    {

        public string ConvertToHtml(string markdownText)
        {
            IParser parser = new Parser();
            IRenderer renderer = new Renderer();
            var parsedMarkdown = parser.Parse(markdownText);
            return renderer.HtmlRender(parsedMarkdown);
        }
    }
}
