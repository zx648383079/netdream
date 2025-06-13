using NetDream.Modules.Shop.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Shop.Market.Models
{
    public class AttributeResult
    {

        public int[] ProductProperties { get; set; } = [];

        public int[] Properties { get; set; } = [];
        /// <summary>
        /// 未排除货品属性之后的价格
        /// </summary>
        public decimal PropertiesPrice { get; set; }

        public string PropertiesLabel { get; set; } = string.Empty;
        /// <summary>
        /// 全部选中属性的价格
        /// </summary>
        public decimal TotalPropertiesPrice { get; set; }

        public ProductEntity? Product { get; set; }
    }

    internal class AttributeFormattedItem
    {
        public decimal Price { get; set; }

        public IList<int> Items { get; set; } = [];
        public IList<string> Label { get; set; } = [];
    }
}
