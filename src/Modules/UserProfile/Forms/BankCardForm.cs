using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.UserProfile.Forms
{
    public class BankCardForm
    {
        public int Id { get; set; }
        [Required]
        public string Bank { get; set; } = string.Empty;
        public byte Type { get; set; }
        [Required]
        public string CardNo { get; set; } = string.Empty;

        public string ExpiryDate { get; set; } = string.Empty;
    }
}
