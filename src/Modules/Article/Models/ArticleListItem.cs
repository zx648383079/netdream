using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Article.Models
{
    public class ArticleListItem : IListArticleItem, IWithUserModel, IWithCategoryModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;

        public int UserId { get; set; }

        public int CatId { get; set; }
        public byte OpenType { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }

        public int ClickCount { get; set; }
        public int CreatedAt { get; set; }



        public IListLabelItem? Category { get; set; }
        public IUser? User { get; set; }

        public bool IsLocalization { get; set; }

        public string Url { get; set; } = string.Empty;
    }
}
