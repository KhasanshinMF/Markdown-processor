using Markdown.Interfaces;
using Markdown.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes
{
    public class Parser : IParser
    {
        public List<MarkdownNode> Parse(string markdownText)
        {
            var markdownNodes = new List<MarkdownNode>();
            var linesOfMarkdownText = markdownText.Split("\r\n");
            foreach (var line in linesOfMarkdownText)
            {
                markdownNodes.Add(ParseLine(line));
            }
            return markdownNodes;
        }

        private MarkdownNode ParseLine(string markdownLine)
        {
            var rootNode = new MarkdownNode(TagType.Root);
            var currentNode = rootNode;

            int i = 0;
            while (i < markdownLine.Length)
            {
                // Парсим заголовки
                if (markdownLine[i] == '#' && (i == 0 || markdownLine[i - 1] == '\n' || markdownLine[i - 1] == '\r'))
                {
                    var (index, node) = HeaderParsing(markdownLine, i);
                    currentNode.AddChild(node);
                    i = index;
                }
                else if (markdownLine[i] == '_')
                {
                    // Парсим жирный текст
                    if (markdownLine[i + 1] == '_')
                    {
                        var (index, node) = BoldParsing(markdownLine, i + 2);
                        currentNode.AddChild(node);
                        i = index;
                    }
                    // Парсим курсив
                    else
                    {
                        var (index, node) = ItalicParsing(markdownLine, i + 1);
                        currentNode.AddChild(node);
                        i = index;
                    }
                }
                // Парсим обычный текст
                else
                {
                    var (index, node) = TextParsing(markdownLine, i);
                    currentNode.AddChild(node);
                    i = index;
                }
            }
            return rootNode;
        }

        private (int, MarkdownNode) HeaderParsing(string markdownText, int index)
        {
            // Считаем уровень заголовка
            int headerLevel = 0;
            while (index < markdownText.Length && markdownText[index] == '#' && headerLevel < 6)
            {
                headerLevel++;
                index++;
            }
            // Пропускаем пробелы между тэгом и текстом
            while (index < markdownText.Length && markdownText[index] == ' ') index++;

            // Текст заголовка
            int endIndex = index;
            while (endIndex < markdownText.Length && markdownText[endIndex] != '\n' && markdownText[endIndex] != '\r') endIndex++;

            return (endIndex, new MarkdownNode(TagType.Header, markdownText.Substring(index, endIndex - index), headerLevel));
        }

        private (int, MarkdownNode) BoldParsing(string markdownText, int index)
        {
            var endIndex = index;
            while (endIndex < markdownText.Length && !(markdownText[endIndex] == '_' && markdownText[endIndex + 1] == '_')) endIndex++;
            

            return (endIndex + 2, new MarkdownNode(TagType.Bold, markdownText.Substring(index, endIndex - index)));
        }

        private (int, MarkdownNode) ItalicParsing(string markdownText, int index)
        {
            var endIndex = index;
            while (endIndex < markdownText.Length && markdownText[endIndex] != '_') endIndex++;

            return (endIndex + 1, new MarkdownNode(TagType.Italic, markdownText.Substring(index, endIndex - index)));
        }

        private (int, MarkdownNode) TextParsing(string markdownText, int index)
        {
            var endIndex = index;
            while (endIndex < markdownText.Length && markdownText[endIndex] != '_') endIndex++;

            return (endIndex, new MarkdownNode(TagType.Text, markdownText.Substring(index, endIndex - index)));
        }
    }
}
