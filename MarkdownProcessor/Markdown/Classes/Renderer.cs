using Markdown.Interfaces;
using Markdown.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes
{
    public class Renderer: IRenderer
    {
        public string HtmlRender(List<MarkdownNode> nodes)
        {
            var htmlText = new StringBuilder();
            foreach (MarkdownNode node in nodes)
            {
                htmlText.AppendLine(LineRender(node));
            }
            htmlText.Remove(htmlText.Length - 2, 2);

            return htmlText.ToString();
        }

        private string LineRender(MarkdownNode node)
        {
            var htmlLine = new StringBuilder();

            foreach (MarkdownNode childNode in node.Children)
            {
                switch (childNode.Type)
                {
                    case TagType.Header:
                        htmlLine.Append($"<h{childNode.HeaderLevel}>{childNode.Text}</h{childNode.HeaderLevel}>");
                        break;
                    case TagType.Bold:
                        htmlLine.Append($"<strong>{childNode.Text}</strong>");
                        break;
                    case TagType.Italic:
                        htmlLine.Append($"<em>{childNode.Text}</em>");
                        break;
                    case TagType.Text:
                        htmlLine.Append(childNode.Text);
                        break;
                }
            }

            return htmlLine.ToString();
        }
    }
}
