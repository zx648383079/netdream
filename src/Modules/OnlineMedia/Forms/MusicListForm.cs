using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.OnlineMedia.Forms
{
    public class MusicListForm
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Cover { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
