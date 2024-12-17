using System;

namespace NetDream.Shared.Interfaces
{
    public interface ICaptchaOutput: IDisposable
    {
        public string ContentType { get; }
        public byte[] AsSpan();
    }
}
