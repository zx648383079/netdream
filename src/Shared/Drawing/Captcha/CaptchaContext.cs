using NetDream.Shared.Interfaces;

namespace NetDream.Shared.Drawing.Captcha
{
    public class TextCaptchaContext: ICaptchaContext
    {
        public int Width { get; set; } = 100;
        public int Height { get; set; } = 30;
        public int Level { get; set; }
    }
}
