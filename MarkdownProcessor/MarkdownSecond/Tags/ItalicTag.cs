using MarkdownSecond.Enums;

namespace MarkdownSecond;

public class ItalicTag : AbstractTag
{
    public override TagType TagType => TagType.Italic;
    public override string MarkdownTag => "_";
    public override string OpenHtmlTag => "<em>";
    public override string CloseHtmlTag => "</em>";
}