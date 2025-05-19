using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.TradeTracker.Forms
{
    public class ProductForm
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string EnName { get; set; } = string.Empty;
        public int CatId { get; set; }
        public int ProjectId { get; set; }
        [Required]
        public string UniqueCode { get; set; } = string.Empty;
        public byte IsSku { get; set; }

        public ChannelProductForm[] Items { get; set; }
    }

    public class ChannelProductForm
    {
        public string Channel { get; set; } = string.Empty;
        public string PlatformNo { get; set; } = string.Empty;
        public string ExtraMeta { get; set; } = string.Empty;
    }
}
