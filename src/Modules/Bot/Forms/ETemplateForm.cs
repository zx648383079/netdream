using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Bot.Forms
{
    public class ETemplateForm
    {
        public int Id { get; set; }
        public byte Type { get; set; }

        public int CatId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
