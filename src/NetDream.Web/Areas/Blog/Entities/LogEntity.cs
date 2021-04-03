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
        [Column("id_value")]
        public int IdValue { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public int Action { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
