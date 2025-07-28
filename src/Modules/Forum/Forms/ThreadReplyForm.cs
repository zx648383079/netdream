using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Forum.Forms
{
    public class ThreadReplyForm
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int Thread { get; set; }
    }
}
