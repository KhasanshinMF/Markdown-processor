using System.Text;
using Markdown.Tags;

namespace Markdown.Renderers
{
    public class Renderer
    {
        public string HtmlRender(List<List<Tag>> parsedMarkdownText)
        {
            var htmlText = new StringBuilder();
            foreach (var parsedLine in parsedMarkdownText)
            {
                htmlText.Append(LineRender(parsedLine));
                htmlText.Append('\n');
            }

            htmlText.Remove(htmlText.Length - 1, 1);
            return htmlText.ToString();
        }

        private string LineRender(List<Tag> parsedLine)
        {
            var htmlLine = new StringBuilder();

            for (int i = 0; i < parsedLine.Count; i++)
            {
                switch (parsedLine[i].Type)
                {
                    case TagType.HeaderOpen:
                        htmlLine.Append($"<h{parsedLine[i].HeaderLevel}>");
                        break;
                    case TagType.HeaderClose:
                        htmlLine.Append($"</h{parsedLine[i].HeaderLevel}>");
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
                        htmlLine.Append(parsedLine[i].Text);
                        break;
                }
            }

            return htmlLine.ToString();
        }
    }
}