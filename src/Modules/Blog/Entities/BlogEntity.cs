using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Repositories.Models;
using NPoco;
namespace NetDream.Modules.Blog.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BlogEntity: IIdEntity, ILanguageEntity, ITimestampEntity
    {
        internal const string ND_TABLE_NAME = "blog";
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
        [Column("programming_language")]
        public string ProgrammingLanguage { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        [Column("edit_type")]
        public int EditType { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("term_id")]
        public int TermId { get; set; }
        public int Type { get; set; }
        [Column("recommend_count")]
        public int RecommendCount { get; set; }
        [Column("comment_count")]
        public int CommentCount { get; set; }
        [Column("click_count")]
        public int ClickCount { get; set; }
        [Column("open_type")]
        public int OpenType { get; set; }
        [Column("open_rule")]
        public string OpenRule { get; set; } = string.Empty;
        [Column("publish_status")]
        public int PublishStatus { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
