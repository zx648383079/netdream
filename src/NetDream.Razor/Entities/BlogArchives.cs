using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Razor.Entities
{
    public class BlogArchives
    {
        public int Year { get; set; }

        public List<BlogEntity> Items = new();

    }
}
