using Markdown.MarkdownProcessor;
using Markdown.Tags;

namespace Markdown.Parsers
{
    public class Parser
    {
        public Document Parse(string markdownText)
        {
            var linesOfMarkdownText = markdownText.Split("\r\n");
            var parsedLines = new List<Line>();

            foreach (var line in linesOfMarkdownText)
            {
                var lineParser = new LineParser();
                var tags = lineParser.ParseLine(line);
                parsedLines.Add(tags);
            }

            return new Document(parsedLines);
        }
    }
}