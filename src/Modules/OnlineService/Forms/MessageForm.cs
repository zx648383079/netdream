using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.OnlineService.Forms
{
    public class MessageForm
    {
        public byte Type { get; set; }
        [Required]
        public string Content { get; set; } = string.Empty;
        public string ExtraRule { get; set; } = string.Empty;
    }
}
