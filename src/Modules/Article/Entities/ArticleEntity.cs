using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Article.Entities
{
    public class ArticleEntity : IIdEntity, ITimestampEntity
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
        public byte Type { get; set; }
        public byte OriginalType { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }

        public int ClickCount { get; set; }

        public byte OpenType { get; set; }

        public string OpenRule { get; set; } = string.Empty;

        public byte PublishStatus { get; set; }
        public byte Status { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
    }
}
