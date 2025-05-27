using System;
using System.IO;

namespace NetDream.Shared.Interfaces
{
    public interface IExporter : IDisposable
    {
        public string FileName { get; }

        public string MimeType { get; }

        public void Write(Stream output);
    }
}
