using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.UserAccount.Models
{
    public class AccountLogListItem : AccountLogEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
