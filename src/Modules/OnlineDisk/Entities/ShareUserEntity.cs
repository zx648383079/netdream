using NPoco;
namespace NetDream.Modules.OnlineDisk.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ShareUserEntity
    {
        internal const string ND_TABLE_NAME = "disk_share_user";
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("share_id")]
        public int ShareId { get; set; }
    }
}
