using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.OnlineMedia.Forms
{
    public class MovieSeriesForm
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int Episode { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
    }
}
