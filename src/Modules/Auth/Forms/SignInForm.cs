using NetDream.Shared.Interfaces.Forms;

namespace NetDream.Modules.Auth.Forms
{
    public class SignInForm
    {

        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Code { get; set; }

        public string? Password { get; set; }


        public string? CaptchaToken { get; set; }
        public string? Captcha { get; set; }

        public string? TwofaCode { get; set; }

        public string? RecoveryCode { get; set; }

        public ISignInForm GetContext()
        {
            if (!string.IsNullOrWhiteSpace(Email))
            {
                return new EmailSignInForm()
                {
                    Email = Email,
                    Password = Password ?? string.Empty,
                };
            }
            if (!string.IsNullOrWhiteSpace(Password))
            {
                return new MobileSignInForm()
                {
                    Mobile = Mobile ?? string.Empty,
                    Password = Password
                };
            }
            return new MobileCodeSignInForm()
            {
                Mobile = Mobile ?? string.Empty,
                Code = Code ?? string.Empty,
            };
        }
    }
}
