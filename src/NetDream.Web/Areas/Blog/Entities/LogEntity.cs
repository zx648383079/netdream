using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Blog.Entities
{
    [TableName("blog_log")]
    public class LogEntity
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public int IdValue { get; set; }
        public int UserId { get; set; }
        public int Action { get; set; }
        public int CreatedAt { get; set; }
    }
}
