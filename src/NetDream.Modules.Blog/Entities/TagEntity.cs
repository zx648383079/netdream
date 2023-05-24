using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Modules.Blog.Entities
{
    [TableName("blog_tag")]
    public class TagEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column("blog_count")]
        public int BlogCount { get; set; }
    }
}
