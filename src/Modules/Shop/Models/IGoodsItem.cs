using NetDream.Modules.Shop.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Shop.Models
{
    public interface IGoodsItem
    {
        public int Id { get; }
        public string Name { get; }
        public string Thumb { get; }
    }

    public class GoodsProperty
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Type { get; set; }

        public IList<GoodsAttributeEntity> AttrItems { get; set; } = [];
    }
    public class GoodsPropertyCollection
    {
        public string Name { get; set; } = string.Empty;

        public IList<GoodsStaticProperty> Items { get; set; } = [];
    }
    public class GoodsStaticProperty
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public GoodsAttributeEntity? AttrItem { get; set; }
        public string Group { get; internal set; } = string.Empty;
    }
}
