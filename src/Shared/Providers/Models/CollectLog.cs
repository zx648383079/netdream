using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Migrations;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Providers.Models
{
    public class CollectLog: IIdEntity, ICreatedEntity
    {
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("item_type")]
        public byte ItemType { get; set; }
        [Column("extra_data")]
        public string ExtraData { get; set; } = string.Empty;
        [Column(MigrationTable.COLUMN_CREATED_AT)]
        public int CreatedAt { get; set; }
    }
}
