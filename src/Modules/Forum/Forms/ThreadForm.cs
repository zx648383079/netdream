using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Forum.Forms
{
    public class ThreadForm
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public int ClassifyId { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public byte IsPrivatePost { get; set; }
    }

    public class ThreadPublishForm
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public int Forum { get; set; }
        public int Classify { get; set; }

        public byte IsPrivatePost { get; set; }
    }
}
