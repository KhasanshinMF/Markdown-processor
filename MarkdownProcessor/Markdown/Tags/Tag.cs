namespace Markdown.Tags
{
    public class Tag
    {
        public int Index { get; }
        public TagType Type { get; }
        public int HeaderLevel { get; }
        public string Text { get; }

        public Tag(TagType type, int headerLevel = 0)
        {
            Type = type;
            HeaderLevel = headerLevel;
        }

        public Tag(int index, TagType type, string text)
        {
            Type = type;
            Index = index;
            Text = text;
        }

        public Tag(int index, TagType type)
        {
            Index = index;
            Type = type;
        }
    }
}