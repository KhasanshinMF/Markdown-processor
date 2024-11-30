// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using System.Xml.Linq;
//
// namespace Markdown.Classes
// {
//     internal class Program
//     {
//         public static void Main(string[] args)
//         {
//             var markdownTest1 = @"# Заголовок 1
// \## Заголовок 2
// \\####### Заголовок 6";
//             
//             // на этом ломается
//             var markdownTest2 = "__жирный, _типа__ курсивный__ должен быть текст__"; 
//             
//             var markdownTest3 = "текст _курсивнный_ полный";
//             var markdownTest4 = "текст _курсивнный неполный";
//             var markdownTest5 = "текст __жирный__ полный";
//             var markdownTest6 = "текст __жирный не полный";
//
//             var markdownTest7 = @"
// ### Жирный и курсив
// 1 __жирный текст__, _курсивный текст_
// 2 _курсивный текст_, __жирный текст__
// 3 _курсивный, но __жирный__ текст_
// 3.1 _курсивный, но __жирный__ текст_
// 4 __жирный, но _курсивный_ текст__
// 5 __жирный, но _курсивный_ и _курсивный_ текст__
// просто #текст";
//             
//             var markdownTest8 = @"
// # Заголовок 1
//
// \## Проверка \экранир\ования
// \##### Заголовок 5
// это _курсивный #текст_, а здесь __жирный текст__
// \\_курсивный текст\\_, а здесь \__жирный текст\__";
//             
//             
//             
//
//             MarkdownProcessor markdownProcessor = new MarkdownProcessor();
//             Console.WriteLine(markdownProcessor.ConvertToHtml(markdownTest7));
//         }
//     }
// }
