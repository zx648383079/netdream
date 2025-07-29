using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Navigation.Forms
{
    public class PageCrawlForm
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Thumb { get; set; }
        [Required]
        public string Link { get; set; }

        public string Keywords { get; set; }
    }
}
