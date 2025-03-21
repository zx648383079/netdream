using NetDream.Modules.Blog.Models;

namespace NetDream.Modules.Blog.Forms
{
    public class BlogBatchForm
    {
        public object? Categories { get; set; }
        public object? NewBlog { get; set; }
        public object? NewComment { get; set; }

        public BlogDetailForm? Detail {  get; set; }
        public BlogRelationForm? Relation {  get; set; }
    }

    public class BlogBatchResult
    {
        public CategoryListItem[]? Categories { get; set; }
        public BlogListItem[]? NewBlog { get; set; }
        public CommentListItem[]? NewComment { get; set; }

        public BlogModel? Detail { get; set; }
        public BlogListItem[]? Relation { get; set; }
    }

    public class BlogDetailForm
    {
        public int Id { get; set; }

        public string? OpenKey { get; set; }
    }

    public class BlogRelationForm
    {
        public int Blog { get; set; }
    }
}
