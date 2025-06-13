using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class GoodsRawForm
    {
        public string Sn { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public string[] Images { get; set; }
    }
}
