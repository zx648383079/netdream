using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.Chat.Models
{
    [TableName("chat_message")]
    public class MessageModel
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public int ReceiveId { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
        public int DeletedAt { get; set; }
        public int CreatedAt { get; set; }
        public int UpdatedAt { get; set; }
    }
}
