using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.Chat.Models
{
    [TableName("chat_friend")]
    public class FriendModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public int BelongId { get; set; }
        public int CreatedAt { get; set; }
        public int UpdatedAt { get; set; }
    }
}
