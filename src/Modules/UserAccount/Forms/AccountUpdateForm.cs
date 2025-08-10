using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.UserAccount.Forms
{
    public class AccountUpdateForm
    {
        [Required]
        public string VerifyType { get; set; }
        [Required]
        public string Verify { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
