using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.MicroBlog.Forms
{
    public class ShareForm
    {
        [Required]
        public string Shareappid { get; set; }
        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public string[] Pics { get; set; }
        public string Content { get; set; }
        public string Sharesource { get; set; }
    }
}
