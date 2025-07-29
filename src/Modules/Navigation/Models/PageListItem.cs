using NetDream.Modules.Navigation.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Navigation.Models
{
    public class PageListItem : IWithUserModel, IWithSiteModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;

        public int SiteId { get; set; }
        public int UserId { get; set; }


        public KeywordEntity[] Keywords { get; set; }
        public IUser? User { get; set; }
        public SiteLabelItem? Site { get; set; }
    }
}
