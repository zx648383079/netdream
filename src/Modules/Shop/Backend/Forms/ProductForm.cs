using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class ProductForm
    {
        public int Id { get; set; }
        [Required]
        public int GoodsId { get; set; }
        public decimal Price { get; set; }

        public decimal MarketPrice { get; set; }
        public int Stock { get; set; }
        public decimal Weight { get; set; }

        public string SeriesNumber { get; set; } = string.Empty;
        public string Attributes { get; set; } = string.Empty;
    }
}
