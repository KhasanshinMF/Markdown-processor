namespace Markdown.MarkdownProcessor
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IMarkdownProcessor markdownProcessor = new MarkdownProcessor();

            var markdownTest1 = "# Header 1";
            Console.WriteLine(markdownProcessor.ConvertToHtml(markdownTest1));
        }
    }
}