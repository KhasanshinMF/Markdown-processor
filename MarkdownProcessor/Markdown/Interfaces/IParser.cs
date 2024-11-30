using Markdown.Classes;

namespace Markdown.Interfaces
{
    public interface IParser
    {
        List<List<Tag>> Parse(string markdownText);
    }
}
