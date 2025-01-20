using System.Text;
using Markdown.MarkdownProcessor;
using Markdown.Tags;

namespace Markdown.Renderers
{
    public class Renderer
    {
        public string ToHtml(Document document)
        {
            var htmlText = new StringBuilder();
            foreach (var line in document.Lines)
            {
                htmlText.Append(RenderLine(line));
                htmlText.Append('\n');
            }

            if (htmlText.Length > 0)
                htmlText.Remove(htmlText.Length - 1, 1);

            return htmlText.ToString();
        }

        private string RenderLine(Line line)
        {
            var htmlLine = new StringBuilder();

            foreach (var tag in line.Tags)
            {
                switch (tag.Type)
                {
                    case TagType.HeaderOpen:
                        htmlLine.Append($"<h{tag.HeaderLevel}>");
                        break;
                    case TagType.HeaderClose:
                        htmlLine.Append($"</h{tag.HeaderLevel}>");
                        break;
                    case TagType.BoldOpen:
                        htmlLine.Append("<strong>");
                        break;
                    case TagType.BoldClose:
                        htmlLine.Append("</strong>");
                        break;
                    case TagType.ItalicOpen:
                        htmlLine.Append("<em>");
                        break;
                    case TagType.ItalicClose:
                        htmlLine.Append("</em>");
                        break;
                    case TagType.Text:
                        htmlLine.Append(tag.Text);
                        break;
                }
            }

            return htmlLine.ToString();
        }
    }
}