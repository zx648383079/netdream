using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.Blog.Entities
{
    [TableName("blog_comment")]
    public class CommentEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public int ParentId { get; set; }
        public int Position { get; set; }
        public int UserId { get; set; }
        public int BlogId { get; set; }
        public string Ip { get; set; }
        public string Agent { get; set; }
        public int Agree { get; set; }
        public int Disagree { get; set; }
        public int Approved { get; set; }
        public int CreatedAt { get; set; }
    }
}
