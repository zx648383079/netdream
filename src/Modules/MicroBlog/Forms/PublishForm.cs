using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.MicroBlog.Forms
{
    public class PublishForm
    {
        [Required]
        public string Content { get; set; }

        public string[] File { get; set; }
    }
}
