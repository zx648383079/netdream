using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Blog.Forms
{
    public class BlogForm
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public string ProgrammingLanguage { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public int EditType { get; set; }
        [Required]
        public string Content { get; set; } = string.Empty;
        public int TermId { get; set; }
        public int Type { get; set; }
        public int OpenType { get; set; }
        public string OpenRule { get; set; } = string.Empty;
        public byte PublishStatus { get; set; }
    }
}
