using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class CouponLogListItem : CouponLogEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
