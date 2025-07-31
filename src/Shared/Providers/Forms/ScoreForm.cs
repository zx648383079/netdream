using System.ComponentModel.DataAnnotations;

namespace NetDream.Shared.Providers.Forms
{
    public class ScoreForm
    {
        [Required]
        public int ItemId { get; set; }
        public byte ItemType { get; set; }
        [Required]
        public byte Score { get; set; }

        public int FromId { get; set; }
        public byte FromType { get; set; }
    }
}
