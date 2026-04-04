using Markdig.Renderers;
using Markdig.Renderers.Html;
using NetDream.Modules.Article.Repositories;
using NetDream.Shared.Helpers;

namespace NetDream.Modules.Blog.Markdown
{
    public class CatalogRenderer(BlogRepository repository) : HtmlObjectRenderer<CatalogBlock>
    {
        protected override void Write(HtmlRenderer renderer, CatalogBlock obj)
        {
            var data = repository.GetList(obj.Items);
            if (data.Length == 0)
            {
                return;
            }
            renderer.Write("<ul class=\"book-catalog-inner\">");
            foreach (var item in data)
            {
                renderer.Write("<li class=\"book-catalog-item\">");
                renderer.Write("<div class=\"item-title\">");
                renderer.Write($"<a class=\"name\" href=\"{item.Url}\">")
                    .WriteEscape(item.Title)
                    .Write("</a>");
                renderer.Write("<div class=\"time\">")
                      .Write(TimeHelper.FormatAgo(item.CreatedAt))
                      .Write("</div>");
                renderer.Write("</div>");
                renderer.Write("<div class=\"item-meta\">")
                    .WriteEscape(item.Description)
                    .Write("</div>");
                renderer.Write("</li>");
            }
            renderer.Write("</ul>");
        }
    }
}
