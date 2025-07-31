using System.IO;

namespace NetDream.Shared.Interfaces
{
    public interface IDownloadFile
    {
        public string Name { get; }
        public string FileType { get; }

        public Stream OpenRead();
    }
}
