using NetDream.Shared.Interfaces;
using SkiaSharp;

namespace NetDream.Shared.Drawing.Captcha
{
    public class ImageCaptchaOutput(SKImage image) : ICaptchaOutput
    {
        public string ContentType => "image/jpeg";

        public byte[] AsSpan()
        {
            return image.Encode(SKEncodedImageFormat.Jpeg, 100).AsSpan().ToArray();
        }

        public void Dispose()
        {
            image.Dispose();
        }
    }
}
