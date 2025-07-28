using NetDream.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Shared.Providers.Forms
{
    public class CommentForm
    {
        [Required]
        public string Content { get; set; } = string.Empty;
        public int Parent { get; set; }
        public int Target { get; set; }
    }

    public class CommentEditForm
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; } = string.Empty;

        public LinkExtraRule[] ExtraRule { get; set; }
        public int ParentId { get; set; }
        public int TargetId { get; set; }
    }
}
