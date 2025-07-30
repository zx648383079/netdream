using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.MicroBlog.Forms
{
    public class CommentForm
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int MicroId { get; set; }
        public int ParentId { get; set; }
        public bool IsForward { get; set; }
    }
}
