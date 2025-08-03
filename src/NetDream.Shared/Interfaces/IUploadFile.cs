using System.Collections.Generic;
using System.IO;

namespace NetDream.Shared.Interfaces
{
    public interface IUploadFile
    {
        public string Name { get; }
        public long Size { get; }
        public string FileType { get; }

        public Stream OpenRead();

        public void CopyTo(Stream output);
    }

    public interface IUploadFileCollection : IEnumerable<IUploadFile>
    {
        public int Count { get; }
    }
}
