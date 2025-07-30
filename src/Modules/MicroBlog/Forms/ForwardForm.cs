using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.MicroBlog.Forms
{
    public class ForwardForm
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        /// <summary>
        /// 是否并评论
        /// </summary>
        public bool IsComment { get; set; }
    }
}
