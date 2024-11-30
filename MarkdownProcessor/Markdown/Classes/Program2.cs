using Markdown.Enums;
using Markdown.TagTree;

namespace Markdown.Classes;

public class Program2
{
    public static void Main(string[] args)
    {
        // TagNode root = new TagNode(new Tag(TagType.HeaderOpen, 1));
        // TagNode current = root;
        //
        // current.Add(new TagNode(new Tag(TagType.Text, "Какой-то текст"), current));
        // var newTag = new TagNode(new Tag(TagType.ItalicOpen), current);
        // current.Add(newTag);
        // current = newTag;
        // current.Add(new TagNode(new Tag(TagType.Text, "Вложенный текст"), newTag));
        // current.Add(new TagNode(new Tag(TagType.Text, "Еще вложенный текст"), newTag));
        //
        // current = newTag.ParentTag;
        // current.Add(new TagNode(new Tag(TagType.ItalicClose), current));
        //
        // current = current.ParentTag;
        // current.Add(new TagNode(new Tag(TagType.HeaderClose, 1)));
        // Console.WriteLine(root.ToString());
        var line = "## Заголовок с _курсивным текстом_ и _еще одним курсивным текстом_";
        LineParser lineParser = new LineParser();
        Console.WriteLine(lineParser.ParseLine(line));
    }
}