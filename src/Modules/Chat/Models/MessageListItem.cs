using NetDream.Modules.Chat.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Chat.Models
{
    public class MessageListItem: MessageEntity, IWithUserModel
    {
        public IUser? User { get; set; }
        public IUser? Receive { get; set; }
    }
}
