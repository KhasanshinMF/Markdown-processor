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
\## Заголовок 2
это _курсивный текст_, а здесь \__жирный\__
\_курсивный текст\_ \__жирный текст\__
_курсивный, но __жир\ный__ текст_
__жирный, но _курсивный_ текст__
просто #текст";
            MarkdownProcessor markdownProcessor = new MarkdownProcessor();
            Console.WriteLine(markdownProcessor.ConvertToHtml(markdownText));
        }
    }
}
