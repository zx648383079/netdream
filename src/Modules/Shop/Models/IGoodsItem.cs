using NetDream.Modules.Shop.Entities;

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
        public string Name { get; }
        public byte Type { get; set; }

        public GoodsAttributeEntity[] AttrItems { get; set; }
    }
    public class GoodsPropertyCollection
    {
        public string Name { get; set; }

        public GoodsStaticProperty[] Items { get; set; }
    }
    public class GoodsStaticProperty
    {
        public int Id { get; set; }
        public string Name { get; }
        public GoodsAttributeEntity AttrItem { get; }
    }
}
