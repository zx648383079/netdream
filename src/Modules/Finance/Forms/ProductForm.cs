using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Finance.Forms
{
    public class ProductForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public byte Status { get; set; }
        public string Remark { get; set; } = string.Empty;
    }
}
