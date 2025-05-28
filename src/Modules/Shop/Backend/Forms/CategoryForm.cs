using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class CategoryForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Banner { get; set; } = string.Empty;

        public string AppBanner { get; set; } = string.Empty;

        public int ParentId { get; set; }
        public int Position { get; set; }
    }
}
