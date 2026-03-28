using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Comment.Forms
{
    public class CommentForm
    {
        [Required]
        public string Content { get; set; } = string.Empty;
        public int Parent { get; set; }
        [Required]
        public int TargetId { get; set; }
        public byte TargetType { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
