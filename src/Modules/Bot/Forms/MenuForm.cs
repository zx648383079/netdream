using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Bot.Forms
{
    public class MenuForm
    {
        public int Id { get; set; }

        public int BotId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Content { get; set; } = string.Empty;

        public int ParentId { get; set; }

        public MenuForm[] Children { get; set; }
    }
}
