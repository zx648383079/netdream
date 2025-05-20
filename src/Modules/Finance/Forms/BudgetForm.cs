using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Finance.Forms
{
    public class BudgetForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public float Budget { get; set; }
        public byte Cycle { get; set; }
    }
}
