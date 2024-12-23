using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.SEO.Forms
{
    public class AgreementForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
