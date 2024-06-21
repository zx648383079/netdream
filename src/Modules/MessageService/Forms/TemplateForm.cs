using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.MessageService.Forms
{
    public class TemplateForm
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string[]? Data { get; set; }
        public string Content { get; set; } = string.Empty;

        [StringLength(255)]
        public string TargetNo { get; set; } = string.Empty;

        public byte Status { get; set; }
    }
}
