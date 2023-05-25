using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Models
{
    public class PasswordForm
    {
        public string VerifyType { get; set; } = string.Empty;
        public string Verify { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;


    }
}
