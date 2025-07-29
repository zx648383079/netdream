using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Navigation.Forms
{
    public class PageForm
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        [Required]
        public string Link { get; set; } = string.Empty;

        public int SiteId { get; set; }
        public byte Score { get; set; }

        public string Keywords { get; set; }
    }
}
