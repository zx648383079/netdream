namespace NetDream.Modules.Navigation.Models
{
    internal interface IWithSiteModel
    {

        public int SiteId { get; }

        public SiteLabelItem? Site { set; }
    }
}