using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class AttributeGroupForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;


        public string PropertyGroups { get; set; } = string.Empty;
    }
}
