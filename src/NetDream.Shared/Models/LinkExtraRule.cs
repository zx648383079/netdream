using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Models
{
    public class LinkExtraRule
    {
        public string S { get; set; } = string.Empty;

        public int U { get; set; }

        public string? I { get; set; }
        public string? L { get; set; }
        public string? F { get; set; }

        public LinkExtraRule()
        {
            
        }

        public LinkExtraRule(string search)
        {
            S = search;
        }

        public LinkExtraRule(string search, int user): this(search)
        {
            U = user;
        }
    }
}
