using SkiaSharp.QrCode.Image;
using System;

namespace NetDream.Shared.Drawing
{
    public class QRCode
    {

        public static string ToBase64String(string text)
        {
            return $"data:image/png;base64,{Convert.ToBase64String(QRCodeImageBuilder.GetPngBytes(text))}";
        }
    }
}
