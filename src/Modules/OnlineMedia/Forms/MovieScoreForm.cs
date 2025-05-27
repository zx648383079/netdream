using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.OnlineMedia.Forms
{
    public class MovieScoreForm
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}
