using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.SEO.Models
{
    [TableName("seo_option")]
    [PrimaryKey("id", AutoIncrement = true)]
    public class OptionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ParentId { get; set; }
        public string Type { get; set; }
        public int Visibility { get; set; }
        [Column("default_value")]
        public string DefaultValue { get; set; }
        public string Value { get; set; }
        public int Position { get; set; }

        public object FormatValue()
        {
            switch (Type)
            {
                case "switch":
                    return Value == "1" || Value == "true";
                case "json":
                    return string.IsNullOrWhiteSpace(Value) ? null : "";
                default:
                    return Value;
            }
        }
    }
}
