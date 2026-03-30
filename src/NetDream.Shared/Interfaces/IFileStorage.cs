using System.Collections.Generic;
using System.IO;

namespace NetDream.Shared.Interfaces
{
    public interface ISystemStorage
    {
        /// <summary>
        /// 外部可以直接网址访问的目录
        /// </summary>
        public IStorageFolder Open { get; }
        /// <summary>
        /// 内部目录，无法直接访问
        /// </summary>
        public IStorageFolder Secret { get; }
        /// <summary>
        /// 临时目录
        /// </summary>
        public IStorageFolder Temporary { get; }
        
    }

    public interface IStorageFolder
    {
        public bool Exist(string fileName);
        public Stream? OpenRead(string fileName);
        public Stream? Create(string fileName);
        public void Create(string fileName, Stream data);

        public void Delete(string fileName);

        public FileInfo? File(string fileName);
        public DirectoryInfo? Directory(string directory);

        public IEnumerable<string> Directories();
        public IEnumerable<string> Directories(string directory);
        public IEnumerable<string> Files();
        public IEnumerable<string> Files(string directory);
        public IEnumerable<string> Files(string directory, string pattern);
    }
}
