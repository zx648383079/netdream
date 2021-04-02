using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Blog.Entities
{
    [TableName("blog")]
    public class BlogEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Thumb { get; set; }
        public string Language { get; set; }
        public int EditType { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int TermId { get; set; }
        public int Type { get; set; }
        public string SourceUrl { get; set; }
        public int Recommend { get; set; }
        public int CommentCount { get; set; }
        public int ClickCount { get; set; }
        public int CommentStatus { get; set; }
        public int DeletedAt { get; set; }
        public int CreatedAt { get; set; }
        public int UpdatedAt { get; set; }
    }
}
