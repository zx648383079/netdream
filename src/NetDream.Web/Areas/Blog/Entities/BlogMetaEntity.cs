using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Blog.Entities
{
    [TableName("blog_meta")]
    public class BlogMetaEntity
    {
        public int Id { get; set; }
        [Column("blog_id")]
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
