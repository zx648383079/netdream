using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class AttributeForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int GroupId { get; set; }
        public byte Type { get; set; }

        public byte SearchType { get; set; }

        public byte InputType { get; set; }

        public string DefaultValue { get; set; } = string.Empty;
        public int Position { get; set; }
        public string PropertyGroup { get; set; } = string.Empty;
    }
}
