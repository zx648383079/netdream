using NetDream.Modules.OnlineService.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.OnlineService.Models
{
    public class MessageListItem: MessageEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
