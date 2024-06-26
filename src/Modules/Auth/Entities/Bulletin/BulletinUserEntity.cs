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
    public class BulletinUserEntity: IIdEntity, ITimestampEntity
    {
        internal const string ND_TABLE_NAME = "bulletin_user";
        public int Id { get; set; }

        [Column("bulletin_id")]
        public int BulletinId { get; set; }

        public byte Status { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("updated_at")]
        public int UpdatedAt { get; set; }

        [Column("created_at")]
        public int CreatedAt { get; set; }

    }
}
