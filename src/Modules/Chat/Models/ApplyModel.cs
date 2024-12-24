using NetDream.Modules.Chat.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Chat.Models
{
    public class ApplyModel: ApplyEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
