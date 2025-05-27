using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.OnlineMedia.Forms
{
    public class MovieForm
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string FilmTitle { get; set; } = string.Empty;
        public string TranslationTitle { get; set; } = string.Empty;
        public string Cover { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public string Leader { get; set; } = string.Empty;
        public string Screenwriter { get; set; } = string.Empty;
        public int CatId { get; set; }
        public int AreaId { get; set; }
        public string Age { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int SeriesCount { get; set; }

        public MovieFileForm[]? Files { get; set; }
    }
}
