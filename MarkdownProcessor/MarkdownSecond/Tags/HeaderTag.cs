using MarkdownSecond.Enums;

namespace MarkdownSecond;

public class HeaderTag : AbstractTag
{
    public override TagType TagType => TagType.Header;
    public override string MarkdownTag => "#";
    public override string OpenHtmlTag => "<h1>";
    public override string CloseHtmlTag => "</h1>";
}