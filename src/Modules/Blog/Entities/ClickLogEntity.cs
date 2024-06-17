using NPoco;
namespace NetDream.Modules.Blog.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BlogClickLogEntity
    {
        internal const string ND_TABLE_NAME = "blog_click_log";
        public int Id { get; set; }
        [Column("happen_day")]
        public string HappenDay { get; set; } = string.Empty;
        [Column("blog_id")]
        public int BlogId { get; set; }
        [Column("click_count")]
        public int ClickCount { get; set; }
    }
}
