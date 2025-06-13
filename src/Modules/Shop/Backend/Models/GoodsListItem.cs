using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class GoodsListItem : IWithCategoryModel, IWithBrandModel
    {
        public int Id { get; set; }

        public int CatId { get; set; }

        public int BrandId { get; set; }
        public string Name { get; set; } = string.Empty;

        public string SeriesNumber { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public decimal MarketPrice { get; set; }
        public int Stock { get; set; }

        public int Sales { get; set; }

        public byte IsBest { get; set; }

        public byte IsHot { get; set; }

        public byte IsNew { get; set; }
        public byte Status { get; set; }

        public byte Type { get; set; }
        public int Position { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
        public ListLabelItem? Category { get; set; }
        public ListLabelItem? Brand { get; set; }
        public ProductEntity[]? Products { get; internal set; }
    }
}
