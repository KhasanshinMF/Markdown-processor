using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Enums;

namespace Markdown.Classes
{
    public class Tag
    {
        public TagType Type { get; }
        public int HeaderLevel { get; }
        public string Text { get; }

        public Tag(TagType type, int headerLevel = 0)
        {
            Type = type;
            HeaderLevel = headerLevel;
        }

        public Tag(TagType type, string text)
        {
            Type = type;
            Text = text;
        }

        public Tag(TagType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return $"{Type}, {HeaderLevel}, {Text}";
        }
    }
}
