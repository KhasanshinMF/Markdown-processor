using System;
using Markdown.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes
{
    public class MarkdownNode
    {
        public TagType Type { get; }
        public int HeaderLevel {  get; }
        public string Text { get; }
        public List<MarkdownNode> Children { get; } = new List<MarkdownNode>();

        public MarkdownNode(TagType type, string text = null, int headerLevel = 0)
        {
            Type = type;
            HeaderLevel = headerLevel;
            Text = text;
        }
        
        public void AddChild(MarkdownNode child)
        {
            Children.Add(child);
        }
    }
}
