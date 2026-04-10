using NetDream.Modules.Article.Models;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Article.Forms
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
        public IListLabelItem[]? Categories { get; set; }
        public ArticleListItem[]? NewBlog { get; set; }
        public ICommentItem[]? NewComment { get; set; }

        public ArticleOpenModel? Detail { get; set; }
        public ArticleListItem[]? Relation { get; set; }
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
