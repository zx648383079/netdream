using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Migrations;
using NPoco;

namespace NetDream.Shared.Providers.Models
{
    public class SketchLog :IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("item_type")]
        public byte ItemType { get; set; }
        public string Data { get; set; } = string.Empty;

        public string Ip { get; set; } = string.Empty;
        [Column(MigrationTable.COLUMN_UPDATED_AT)]
        public int UpdatedAt { get; set; }
        [Column(MigrationTable.COLUMN_CREATED_AT)]
        public int CreatedAt { get; set; }
    }
}
