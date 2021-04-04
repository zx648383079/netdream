using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Razor.Entities
{
    [TableName("blog_tag")]
    public class TagEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column("blog_count")]
        public int BlogCount { get; set; }

        [Ignore]
        public string FontSize
        {
            get
            {
                return (BlogCount + 12) + "px";
            }
        }
    }
}
