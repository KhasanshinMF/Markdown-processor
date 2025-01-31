namespace Markdown.MarkdownProcessor
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IMarkdownProcessor markdownProcessor = new MarkdownProcessor();
            
            var markdownTest1 = @"
# Header 1
## Заголовок 2 уровня
#######__жирный и _курсивный_ текст__
_курсивный и __жирный текст__
\_экранированный курсив\_
просто какой-то #текст";
            Console.WriteLine(markdownProcessor.ConvertToHtml(markdownTest1));
        }
    }
}