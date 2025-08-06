using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Bot.Forms
{
    public class CategoryForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        public int ParentId { get; set; }
    }
}
