using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Catering.Forms
{
    public class PatronGroupForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public byte Discount { get; set; }
    }
}
