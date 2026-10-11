using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Bot.Forms
{
    public class ReplyForm
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Event { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public byte Match { get; set; }
        public string Content { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string StartAt { get; set; } = string.Empty;
        public string EndAt { get; set; } = string.Empty;
        public byte Status { get; set; }
    }
}
