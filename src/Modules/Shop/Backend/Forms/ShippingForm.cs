using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class ShippingForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
        public int Method { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Position { get; set; }

        public ShippingGroupForm[]? Groups { get; set; }
    }
}
