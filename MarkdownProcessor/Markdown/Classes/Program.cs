using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Markdown.Classes
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var markdownText = @"
# Заголовок 1
## Заголовок 2
это _курсивный текст_, а здесь __жирный__
__жирный текст__
_курсивный, но __жирный__ текст_
__жирный, но _курсивный_ текст__
просто #текст
# Заголовок с _разными_ символами";

            //var markdownText = "# заголовок";

            MarkdownProcessor markdownProcessor = new MarkdownProcessor();
            Console.Write(markdownProcessor.ConvertToHtml(markdownText));
            Console.WriteLine("Заголовок");
        }
    }
}
