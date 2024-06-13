using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Models
{
    public class LoginForm
    {
        public string Mobile { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string CaptchaToken { get; set; } = string.Empty;

        public string Captcha { get; set; } = string.Empty;

        public bool Remember { get; set; }
    }
}
