using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Chat.Models
{
    [TableName("chat_friend_classify")]
    public class FriendGroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
