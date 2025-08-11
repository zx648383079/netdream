using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Auth.Forms
{
    public class ResetPasswordForm
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }

    public class UpdatePasswordForm
    {
        public string VerifyType { get; set; }
        public string Verify { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
