using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Models
{
    public class OptionItem(string name, object value)
    {
        public string Name { get; set; } = name;

        public object Value { get; set; } = value;
    }
}
