using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Chat.Models
{
    [TableName("chat_message")]
    public class MessageModel
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        [Column("extra_rule")]
        public string ExtraRule { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("receive_id")]
        public int ReceiveId { get; set; }
        [Column("group_id")]
        public int GroupId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public int Status { get; set; }
        [Column("deleted_at")]
        public int DeletedAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
