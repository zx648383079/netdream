using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Modules.UserAccount.Models
{
    public class ApplyListItem : ApplyLogEntity, IApplyListItem
    {
        public IUser? User { get; set; }

        ReviewStatus IApplyListItem.Status => (ReviewStatus)Status;
    }
}
