using NetDream.Shared.Interfaces.Entities;
using NPoco;
namespace NetDream.Modules.Blog.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BlogLogEntity: IActionEntity
    {
        internal const string ND_TABLE_NAME = "blog_log";
        public int Id { get; set; }
        [Column("item_type")]
        public byte ItemType { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public byte Action { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
