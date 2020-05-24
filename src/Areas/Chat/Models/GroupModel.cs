using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.Chat.Models
{
    [TableName("chat_group")]
    public class GroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int CreatedAt { get; set; }
        public int UpdatedAt { get; set; }
    }
}
