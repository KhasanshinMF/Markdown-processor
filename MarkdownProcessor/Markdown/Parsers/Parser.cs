using Markdown.Tags;

namespace Markdown.Parsers
{
    public class Parser
    {
        public List<List<Tag>> Parse(string markdownText)
        {
            var parsedMarkdownText = new List<List<Tag>>();
            var linesOfMarkdownText = markdownText.Split("\r\n");

            foreach (var line in linesOfMarkdownText)
            {
                var lineParser = new LineParser(line);
                parsedMarkdownText.Add(lineParser.ParseLine());
            }

            return parsedMarkdownText;
        }
    }
}