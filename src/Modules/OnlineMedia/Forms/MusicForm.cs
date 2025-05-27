using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.OnlineMedia.Forms
{
    public class MusicForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Cover { get; set; } = string.Empty;
        public string Album { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public int Duration { get; set; }
    }
}
