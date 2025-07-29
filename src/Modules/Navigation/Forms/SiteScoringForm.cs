using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Navigation.Forms
{
    public class SiteScoringForm
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public byte Score { get; set; }

        public string ChangeReason { get; set; }
    }
}
