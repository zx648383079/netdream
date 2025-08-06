using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Bot.Forms
{
    public class GroupForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        public int BotId { get; set; }

        public string TagId { get; set; } = string.Empty;
    }
}
