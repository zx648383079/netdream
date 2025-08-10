using NetDream.Shared.Interfaces.Forms;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Auth.Forms
{
    public class SignUpForm
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Mobile { get; set; }
        public string? Code { get; set; }
        public string? Email { get; set; }
        [Compare(nameof(ConfirmPassword))]
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string InviteCode { get; set; } = string.Empty;
        public bool Agree { get; set; }

        public ISignUpForm GetContext()
        {
            if (!string.IsNullOrWhiteSpace(Email))
            {
                return new EmailSignUpForm()
                {
                    Name = Email,
                    Email = Email,
                    Password = Password ?? string.Empty,
                    ConfirmPassword = ConfirmPassword,
                    InviteCode = InviteCode,
                    Agree = Agree
                };
            }
            return new MobileSignUpForm()
            {
                Name = Name,
                Mobile = Mobile ?? string.Empty,
                Code = Code ?? string.Empty,
                Password = Password ?? string.Empty,
                ConfirmPassword = ConfirmPassword,
                InviteCode = InviteCode,
                Agree = Agree
            };
        }
    }
}
