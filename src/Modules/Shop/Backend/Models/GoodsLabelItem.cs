using NetDream.Modules.Shop.Models;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class GoodsLabelItem : IGoodsItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SeriesNumber { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
    }
}
