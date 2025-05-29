using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Models;
using NetDream.Shared.Converters;
using System.Collections.Generic;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class GoodsModel : GoodsEntity
    {
        public GoodsProperty[] Properties { get; internal set; }
        public GoodsPropertyCollection[] StaticProperties { get; internal set; }
        public CategoryEntity? Category { get; internal set; }
        public BrandEntity? Brand { get; internal set; }
        public ProductEntity[] Products { get; internal set; }
        public GoodsGalleryEntity[] Gallery { get; internal set; }

        [JsonMeta]
        public Dictionary<string, string>? MetaItems { get; set; }
    }
}
