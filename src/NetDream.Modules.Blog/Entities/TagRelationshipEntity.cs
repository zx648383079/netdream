using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Modules.Blog.Entities
{
    [TableName("blog_tag_relationship")]
    public class TagRelationshipEntity
    {
        [Column("tag_id")]
        public int TagId { get; set; }
        [Column("blog_id")]
        public int BlogId { get; set; }
        public int Position { get; set; }
    }
}
