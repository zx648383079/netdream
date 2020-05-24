using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.Blog.Entities
{
    [TableName("blog_term")]
    public class CategoryEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string Thumb { get; set; }
        public string Styles { get; set; }
    }
}
