using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.UserAccount.Models
{
    public class AdminLogListItem : AdminLogEntity, IWithUserModel
    {
        public IUser? User {  get; set; }
    }
}
