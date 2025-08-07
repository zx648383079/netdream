using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.UserAccount.Forms
{
    public class RechargeForm
    {
        [Required]
        public int User { get; set; }
        [Required]
        public int Money { get; set; }
        [Required]
        public string Remark { get; set; } = string.Empty;
        public bool IsMinus { get; set; }

        public int Type {
            set {
                IsMinus = value > 0;
            }
            get {
                return IsMinus ? 1 : 0;
            }
        }
    }
}
