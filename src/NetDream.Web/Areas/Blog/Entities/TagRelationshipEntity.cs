using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Blog.Entities
{
    [TableName("blog_tag_relationship")]
    public class TagRelationshipEntity
    {
        public int TagId { get; set; }
        public int BlogId { get; set; }
        public int Position { get; set; }
    }
}
