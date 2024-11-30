// using Markdown.Enums;
// using Markdown.Interfaces;
//
// namespace Markdown.Classes
// {
//     public class Parser : IParser
//     {
//         public List<List<Tag>> Parse(string markdownText)
//         {
//             var parsedMarkdownText = new List<List<Tag>>();
//             var linesOfMarkdownText = markdownText.Split("\r\n");
//
//             foreach (var line in linesOfMarkdownText)
//             {
//                 LineParser lineParser = new LineParser();
//                 parsedMarkdownText.Add(lineParser.ParseLine(line));
//             }
//
//             return parsedMarkdownText;
//         }
//     }
// }