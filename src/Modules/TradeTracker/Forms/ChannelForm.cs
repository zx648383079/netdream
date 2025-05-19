using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.TradeTracker.Forms
{
    public class ChannelForm
    {
        public int Id { get; set; }
        [Required]
        public string ShortName { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        public string SiteUrl { get; set; } = string.Empty;
    }
}
