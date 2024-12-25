using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Book.Forms
{
    public class AuthorForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
