using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.UserProfile.Forms
{
    public class AddressForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public int RegionId { get; set; }

        public string? RegionName { get; set; }

        [Required]
        public string Tel { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;

        public bool IsDefault { get; set; }
    }
}
