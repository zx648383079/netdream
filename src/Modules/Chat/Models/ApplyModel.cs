using NetDream.Modules.Chat.Entities;
using NetDream.Shared.Interfaces.Entities;
using NPoco;

namespace NetDream.Modules.Chat.Models
{
    public class ApplyModel: ApplyEntity, IWithUserModel
    {
        [Ignore]
        public IUser? User { get; set; }
    }
}
