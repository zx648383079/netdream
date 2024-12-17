using NetDream.Modules.Auth.Forms;
using NetDream.Shared.Drawing.Captcha;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Auth.Repositories
{
    public class CaptchaRepository
    {

        public ICaptchaOutput Generate(CaptchaForm form)
        {
            ICaptcha instance = form.Type switch
            {
                "hint" => new HintCaptcha(form),
                "slide" => new SlideCaptcha(form),
                _ => new TextCaptcha(form),
            };
            
            return instance.Generate();
        }

    }
}
