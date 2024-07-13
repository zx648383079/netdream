using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.OnlineService.Forms
{
    public class CategoryWordForm
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public int CatId { get; set; }
    }
}
