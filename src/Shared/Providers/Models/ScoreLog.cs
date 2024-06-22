using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Migrations;
using NPoco;

namespace NetDream.Shared.Providers.Models
{
    public class ScoreLog: IIdEntity, ICreatedEntity
    {
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("item_type")]
        public byte ItemType { get; set; }
        public byte Score { get; set; }

        [Column("from_id")]
        public int FromId { get; set; }
        [Column("from_type")]
        public byte FromType { get; set; }

        [Column(MigrationTable.COLUMN_CREATED_AT)]
        public int CreatedAt { get; set; }
    }
}
