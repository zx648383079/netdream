using Markdig;
using NetDream.Modules.Blog.Repositories;

namespace NetDream.Modules.Blog.Markdown
{
    public static class MarkdownExtensions
    {
        public static MarkdownPipelineBuilder UseNetDreamExtensions(this MarkdownPipelineBuilder pipeline, BlogRepository repository)
        {
            pipeline.Extensions.Add(new NetDreamExtension(repository));
            return pipeline;
        }
    }
}
