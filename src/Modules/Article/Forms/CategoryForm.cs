using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Article.Forms
{
    public class CategoryForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public string Keywords { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
    }
}
