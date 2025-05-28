using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Models;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class ActivityModel : ActivityEntity
    {
        public new IActivityConfigure Configure { get; set; }
    }
}
