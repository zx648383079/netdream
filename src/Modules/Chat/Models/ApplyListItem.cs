using NetDream.Modules.Chat.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Chat.Models
{
    public class ApplyListItem: ApplyEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
