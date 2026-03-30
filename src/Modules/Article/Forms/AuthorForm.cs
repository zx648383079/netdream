using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Article.Forms
{
    public class AuthorForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
