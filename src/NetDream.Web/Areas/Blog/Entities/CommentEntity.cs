using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Blog.Entities
{
    [TableName("blog_comment")]
    public class CommentEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        [Column("parent_id")]
        public int ParentId { get; set; }
        public int Position { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("blog_id")]
        public int BlogId { get; set; }
        public string Ip { get; set; }
        public string Agent { get; set; }
        [Column("agree_count")]
        public int AgreeCount { get; set; }
        [Column("disagree_count")]
        public int DisagreeCount { get; set; }
        public int Approved { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
