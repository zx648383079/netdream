using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Shop.Market.Forms
{
    public class CommentForm
    {
        public int ItemType { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        public byte Rank { get; set; }

        public string[] Images { get; set; }
    }
}
