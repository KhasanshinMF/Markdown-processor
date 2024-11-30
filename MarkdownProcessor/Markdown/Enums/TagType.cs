using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Enums
{
    public enum TagType
    {
        Root,
        HeaderOpen,
        HeaderClose,
        BoldOpen,
        BoldClose,
        ItalicOpen,
        ItalicClose,
        Text
    }
}
