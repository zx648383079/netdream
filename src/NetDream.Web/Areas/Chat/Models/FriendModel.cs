using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Chat.Models
{
    [TableName("chat_friend")]
    public class FriendModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Column("classify_id")]
        public int ClassifyId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("belong_id")]
        public int BelongId { get; set; }
        public int Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
