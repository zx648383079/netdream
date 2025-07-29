using NetDream.Modules.Navigation.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Navigation.Models
{
    public class PageModel : PageEntity, IWithSiteModel, IWithUserModel
    {
        public KeywordEntity[] Keywords { get; set; }
        public IUser? User { get; set; }
        public SiteLabelItem? Site { get; set; }
    }
}
