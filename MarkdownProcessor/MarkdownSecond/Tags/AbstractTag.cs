using MarkdownSecond.Enums;

namespace MarkdownSecond;

public abstract class AbstractTag
{
    public abstract TagType TagType { get; }
    public abstract string MarkdownTag { get; }
    public abstract string OpenHtmlTag { get; }
    public abstract string CloseHtmlTag { get; }
}