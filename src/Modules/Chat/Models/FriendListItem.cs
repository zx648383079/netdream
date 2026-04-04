using NetDream.Modules.Chat.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Chat.Models
{
    public class FriendListItem: FriendEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
