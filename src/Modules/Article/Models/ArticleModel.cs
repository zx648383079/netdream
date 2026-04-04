using NetDream.Modules.Article.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Article.Models
{
    public class ArticleModel : IArticle, IWithUserModel, IWithCategoryModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;

        public int ParentId { get; set; }

        public string Language { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;

        public byte EditType { get; set; }
        public string Content { get; set; } = string.Empty;

        public int UserId { get; set; }

        public int CatId { get; set; }
        public byte OriginalType { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }

        public int ClickCount { get; set; }

        public byte OpenType { get; set; }

        public string OpenRule { get; set; } = string.Empty;

        public int PublishStatus { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }

        public IListLabelItem? Category { get; set; }
        public IUser? User { get; set; }
        public bool IsLocalization { get; set; }

        public bool IsLiked { get; set; }


        public ArticleModel()
        {
            
        }

        public ArticleModel(ArticleEntity entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Keywords = entity.Keywords;
            Thumb = entity.Thumb;
            Description = entity.Description;
            EditType = entity.EditType;
            OriginalType = entity.OriginalType;
            Content = entity.Content;
            OpenType = entity.OpenType;
            Language = entity.Language;
            UserId = entity.UserId;
            CatId = entity.CatId;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            LikeCount = entity.LikeCount;
            CommentCount = entity.CommentCount;
            ClickCount = entity.ClickCount;
            OpenRule  = entity.OpenRule;
            PublishStatus = entity.PublishStatus;
            ParentId = entity.ParentId;
        }
    }
}
