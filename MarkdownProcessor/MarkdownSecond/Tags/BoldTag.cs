using MarkdownSecond.Enums;

namespace MarkdownSecond;

public class BoldTag : AbstractTag
{
    public override TagType TagType => TagType.Bold;
    public override string MarkdownTag => "__";
    public override string OpenHtmlTag => "<strong>";
    public override string CloseHtmlTag => "</strong>";
}