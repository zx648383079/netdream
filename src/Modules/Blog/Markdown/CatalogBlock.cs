using Markdig.Syntax.Inlines;

namespace NetDream.Modules.Blog.Markdown
{
    public class CatalogBlock(int[] items) : LeafInline
    {

        public int[] Items => items;
    }
}
