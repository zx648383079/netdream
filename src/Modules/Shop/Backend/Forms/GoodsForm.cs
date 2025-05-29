using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class GoodsForm
    {
        public int Id { get; set; }

        public int CatId { get; set; }

        public int BrandId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        public string SeriesNumber { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Brief { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public float Price { get; set; }
        public float CostPrice { get; set; }
        public float MarketPrice { get; set; }
        public int Stock { get; set; }

        public int AttributeGroupId { get; set; }
        public float Weight { get; set; }

        public int ShippingId { get; set; }
        public byte Sales { get; set; }

        public byte IsBest { get; set; }

        public byte IsHot { get; set; }

        public byte IsNew { get; set; }

        public string AdminNote { get; set; } = string.Empty;
        public byte Type { get; set; }
        public int Position { get; set; }

        public int DeletedAt { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
        public GoodsAttributeForm[] Attr { get; internal set; }
        public ProductForm[] Products { get; internal set; }
        public GoodsGalleryForm[] Gallery { get; internal set; }
    }

    public class GoodsAttributeForm
    {
        public int Id { get; set; }

        public int GoodsId { get; set; }
        [Required]
        public int AttributeId { get; set; }
        [Required]
        public string Value { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class GoodsGalleryForm
    {
        public int Id { get; set; }

        public int GoodsId { get; set; }
        [Required]
        public string File { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public int Type { get; set; }
    }
}
