using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.Open.Entities
{
    [TableName("open_user_token")]
    public class UserTokenEntity
    {
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("platform_id")]
        public int PlatformId { get; set; }
        public string Token { get; set; }
        [Column("expired_at")]
        public int ExpiredAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
    }
}
