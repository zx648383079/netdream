using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.OnlineMedia.Forms
{
    public class LiveForm
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
    }
}
