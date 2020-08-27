using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.Auth.Entities
{
    [TableName("user")]
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Sex { get; set; }
        public string Birthday { get; set; }
        public string Avatar { get; set; }
        public int Money { get; set; }
        public string Token { get; set; }
        public int Status { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
    }
}
