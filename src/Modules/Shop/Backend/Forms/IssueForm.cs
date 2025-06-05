using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class IssueForm
    {
        public int Id { get; set; }
        [Required]
        public int GoodsId { get; set; }
        [Required]
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        public byte Status { get; set; }
    }
}
