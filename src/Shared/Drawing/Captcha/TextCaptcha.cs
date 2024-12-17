using NetDream.Shared.Interfaces;
using SkiaSharp;
using System;

namespace NetDream.Shared.Drawing.Captcha
{
    public class TextCaptcha(ICaptchaContext context) : ICaptcha
    {
        public bool IsOnlyImage => true;

        public ICaptchaOutput Generate()
        {
            if (context.Width <= 0)
            {
                context.Width = 100;
            }
            if (context.Height <= 0) 
            {
                context.Height = 30;
            }
            var info = new SKImageInfo(context.Width, context.Height);
            using var image = SKSurface.Create(info);
            using var canvas = image.Canvas;
            canvas.Clear(SKColors.AliceBlue);
            return new ImageCaptchaOutput(image.Snapshot());
        }

        public bool Verify(object value, object source)
        {
            throw new NotImplementedException();
        }
    }
}
