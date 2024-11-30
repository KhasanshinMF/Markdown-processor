using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Enums;
using Markdown.Interfaces;

namespace Markdown.Classes
{
    public class Renderer: IRenderer
    {
        public string HtmlRender(List<List<Tag>> parsedMarkdownText)
        {
            var htmlText = new StringBuilder();
            foreach (var parsedLine in parsedMarkdownText)
            {
                htmlText.Append(LineRender(parsedLine));
                htmlText.Append('\n');
            }

            return htmlText.ToString();
        }

        private string LineRender(List<Tag> parsedLine)
        {
            var htmlLine = new StringBuilder();

            for (int i = 0; i < parsedLine.Count; i++)
            {
                switch (parsedLine[i].Type)
                {
                    case Enums.TagType.HeaderOpen:
                        htmlLine.Append($"<h{parsedLine[i].HeaderLevel}>");
                        break;
                    case Enums.TagType.HeaderClose:
                        htmlLine.Append($"</h{parsedLine[i].HeaderLevel}>");
                        break;
                    case Enums.TagType.BoldOpen:
                        htmlLine.Append("<strong>");
                        break;
                    case Enums.TagType.BoldClose:
                        htmlLine.Append("</strong>");
                        break;
                    case Enums.TagType.ItalicOpen:
                        htmlLine.Append("<em>");
                        break;
                    case Enums.TagType.ItalicClose:
                        htmlLine.Append("</em>");
                        break;
                    case Enums.TagType.Text:
                        htmlLine.Append(parsedLine[i].Text);
                        break;
                }
            }

            return htmlLine.ToString();
        }
    }
}
