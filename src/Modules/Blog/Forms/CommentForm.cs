using System.ComponentModel.DataAnnotations;

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
