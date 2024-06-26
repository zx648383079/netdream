using NetDream.Shared.Interfaces.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BulletinEntity: IIdEntity, ITimestampEntity
    {
        internal const string ND_TABLE_NAME = "bulletin";
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        [Column("extra_rule")]
        public string ExtraRule { get; set; } = string.Empty;

        public byte Type { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("updated_at")]
        public int UpdatedAt { get; set; }

        [Column("created_at")]
        public int CreatedAt { get; set; }

    }
}
