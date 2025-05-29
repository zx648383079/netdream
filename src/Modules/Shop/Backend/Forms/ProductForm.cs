using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class ProductForm
    {
        public int Id { get; set; }
        [Required]
        public int GoodsId { get; set; }
        public float Price { get; set; }

        public float MarketPrice { get; set; }
        public int Stock { get; set; }
        public float Weight { get; set; }

        public string SeriesNumber { get; set; } = string.Empty;
        public string Attributes { get; set; } = string.Empty;
    }
}
