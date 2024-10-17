using NetDream.Modules.Chat.Entities;
using NetDream.Shared.Interfaces.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Chat.Models
{
    public class HistoryModel: HistoryEntity
    {
        [Ignore]
        public MessageEntity? Message { get; set; }
        [Ignore]
        public IUser? User { get; set; }
        [Ignore]
        public FriendEntity? Friend { get; set; }
        [Ignore]
        public GroupEntity? Group { get; set; }
    }
}
