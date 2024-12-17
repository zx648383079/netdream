using NetDream.Shared.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetDream.Modules.Auth.Forms
{
    public class CaptchaForm: ICaptchaContext
    {
        public int Level { get; set; }

        [Column("captcha_token")]
        public string Token { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
