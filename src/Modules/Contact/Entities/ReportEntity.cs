using NetDream.Shared.Interfaces.Entities;
using NPoco;
namespace NetDream.Modules.Contact.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ReportEntity: IIdEntity, ITimestampEntity
    {
        internal const string ND_TABLE_NAME = "cif_report";
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        [Column("item_type")]
        public byte ItemType { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        public byte Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Files { get; set; } = string.Empty;
        public int Status { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Ip { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
