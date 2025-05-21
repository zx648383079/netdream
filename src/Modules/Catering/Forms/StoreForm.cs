using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Catering.Forms
{
    public class StoreForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public byte OpenStatus { get; set; }
    }
}
