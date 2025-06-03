using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.UserProfile.Forms
{
    public class CertificationForm
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public byte Type { get; set; }
        [Required]
        public string CardNo { get; set; } = string.Empty;
        [Required]
        public string ExpiryDate { get; set; } = string.Empty;
        public string Profession { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string FrontSide { get; set; } = string.Empty;
        [Required]
        public string BackSide { get; set; } = string.Empty;
    }
}
