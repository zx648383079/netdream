using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace NetDream.Modules.Legwork.Forms
{
    public class ProviderForm
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public JsonNode[] Categories { get; set; }
    }
}
