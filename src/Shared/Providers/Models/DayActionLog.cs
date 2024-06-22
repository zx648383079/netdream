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
    public class DayActionLog: IIdEntity, ICreatedEntity
    {
        public int Id { get; set; }
        [Column("happen_day")]
        public string HappenDay { get; set; } = string.Empty;
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("item_type")]
        public byte ItemType { get; set; }
        public byte Action { get; set; }

        [Column("happen_count")]
        public int HappenCount { get; set; }

        [Column(MigrationTable.COLUMN_CREATED_AT)]
        public int CreatedAt { get; set; }
    }
}
