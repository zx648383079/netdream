using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.UserAccount.Models
{
    public class ActionLogListItem : ActionLogEntity, IWithUserModel
    {
        public IUser? User {  get; set; }
    }
}
