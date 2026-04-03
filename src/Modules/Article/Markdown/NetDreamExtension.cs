using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using NetDream.Modules.Blog.Repositories;

namespace NetDream.Modules.Blog.Markdown
{
    public class NetDreamExtension(BlogRepository repository) : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.InlineParsers.Contains<CatalogParser>())
            {
                pipeline.InlineParsers.Insert(0, new CatalogParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                if (!htmlRenderer.ObjectRenderers.Contains<CatalogRenderer>())
                {
                    htmlRenderer.ObjectRenderers.Insert(0, new CatalogRenderer(repository));
                }
                // 替换默认的代码块渲染器
                var originalRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>();
                if (originalRenderer != null)
                {
                    htmlRenderer.ObjectRenderers.Remove(originalRenderer);
                }
                if (!htmlRenderer.ObjectRenderers.Contains<SourceCodeTraceRenderer>())
                {
                    htmlRenderer.ObjectRenderers.Insert(0, new SourceCodeTraceRenderer());
                }
            }
        }
    }
}
