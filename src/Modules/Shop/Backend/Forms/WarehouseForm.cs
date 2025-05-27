using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class WarehouseForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;

        public string LinkUser { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;

        public int[]? Region { get; set; }
    }
}
