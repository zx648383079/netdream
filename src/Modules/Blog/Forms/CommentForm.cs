using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Blog.Forms
{
    public class CommentForm
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; } = string.Empty;
        public string ExtraRule { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public int BlogId { get; set; }
    }
}
