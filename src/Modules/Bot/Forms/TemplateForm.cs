using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Bot.Forms
{
    public class TemplateForm
    {
        public int Id { get; set; }
        public string TemplateId { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        public string Example { get; set; } = string.Empty;
    }
}
