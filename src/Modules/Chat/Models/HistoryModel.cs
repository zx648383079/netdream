using NetDream.Modules.Chat.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Chat.Models
{
    public class HistoryModel: HistoryEntity
    {
        public MessageEntity? Message { get; set; }
        public IUser? User { get; set; }
        public FriendEntity? Friend { get; set; }
        public GroupEntity? Group { get; set; }
    }
}
