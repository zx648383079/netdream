using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Finance.Forms
{
    public class AccountForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public decimal Money { get; set; }
        public decimal FrozenMoney { get; set; }
        public byte Status { get; set; }
        public string Remark { get; set; } = string.Empty;
    }
}
