using System.Text;
using Markdown.Classes;
using Markdown.Enums;

namespace Markdown.TagTree;

public class TagNode
{
    public Tag Tag { get; set; }
    public TagNode ParentTag { get; set; }
    public List<TagNode> ChildTags { get; set; } = new List<TagNode>();

    public TagNode(Tag tag)
    {
        Tag = tag;
    }

    public TagNode(Tag tag, TagNode parentTag)
    {
        Tag = tag;
        ParentTag = parentTag;
    }

    public void Add(TagNode tagNode)
    {
        ChildTags.Add(tagNode);
    }

    public override string ToString()
    {
        var tree = new StringBuilder();
        tree.Append($"{Tag}\n");
        if (ChildTags.Count != 0)
        {
            tree.Append("{\n");
            foreach (var child in ChildTags)
            {
                tree.Append(child.ToString());
            }
            tree.Append("}\n");
        }
        
        return tree.ToString();
    }
}

