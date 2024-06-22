using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Providers.Models
{
    public class ScoreCount
    {
        public byte Score { get; set; }

        public int Count { get; set; }
    }
    public class ScoreOptionItem
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public int Count { get; set; }

        public ScoreOptionItem(string name, string label, int count)
        {
            Name = name;
            Label = label;
            Count = count;
        }

        public ScoreOptionItem(string name, int count)
        {
            Name = name;
            Label = name switch
            {
                "good" => "好评",
                "middle" => "一般",
                "bad" => "差评",
                _ => ""
            };
            Count = count;
        }
    }

    public class ScoreSubtotal
    {
        public int Total { get; set; }
        public int Good { get; set; }
        public int Middle { get; set; }
        public int Bad { get; set; }
        public float Avg { get; set; }

        public float FavorableRate { get; set; }

        public IList<ScoreOptionItem> TagItems 
        {
            get {
                var items = new List<ScoreOptionItem>();
                if (Good > 0)
                {
                    items.Add(new("good", Good));
                }
                if (Middle > 0)
                {
                    items.Add(new("middle", Middle));
                }
                if (Bad > 0)
                {
                    items.Add(new("bad", Bad));
                }
                return items;
            }
        }
    }
}
