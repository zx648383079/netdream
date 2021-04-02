using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Gzo.Models
{
    public class ColumnItem
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string ToType()
        {
            switch (Type)
            {
                case "tinyint":
                case "smallint":
                    return "int";
                case "char":
                case "varchar":
                case "text":
                    return "string";
                case "date":
                    return "string";
                default:
                    return Type;
            }
        }
    }
}
