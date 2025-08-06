using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Bot.Forms
{
    public class ReplyForm
    {
        public int Id { get; set; }
        [Required]
        public string Event { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public byte Match { get; set; }
        public string Content { get; set; } = string.Empty;
        public byte Type { get; set; }
    }
}
