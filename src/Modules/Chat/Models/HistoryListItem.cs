using NetDream.Modules.Chat.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Chat.Models
{
    public class HistoryListItem : HistoryEntity
    {
        public MessageLabelItem? Message { get; set; }
        public IUser? User { get; set; }
        public FriendLabelItem? Friend { get; set; }
        public GroupLabelItem? Group { get; set; }
    }
}
