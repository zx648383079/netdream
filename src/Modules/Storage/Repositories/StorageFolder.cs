using NetDream.Shared.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace NetDream.Modules.Storage.Repositories
{
    public class StorageFolder(string folder) : IStorageFolder
    {

        private bool TryCombine(string fileName, out string fullPath)
        {
            if (fileName.StartsWith(folder))
            {
                fileName = fileName[folder.Length..];
            }
            fullPath = Path.GetFullPath(Path.Combine(folder, fileName));
            return fullPath.StartsWith(folder);
        }

        public bool Exist()
        {
            return System.IO.Directory.Exists(folder);
        }
        public void Create()
        {
            System.IO.Directory.CreateDirectory(folder);
        }

        public void Create(string fileName, Stream data)
        {
            using var fs = Create(fileName);
            if (fs is null)
            {
                return;
            }
            data.CopyTo(fs);
        }

        public void Delete(string fileName)
        {
            if (!TryCombine(fileName, out var path))
            {
                return;
            }
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        public IEnumerable<string> Directories()
        {
            return System.IO.Directory.EnumerateDirectories(folder);
        }

        public IEnumerable<string> Directories(string directory)
        {
            if (TryCombine(directory, out var path) && System.IO.Directory.Exists(path))
            {
                return System.IO.Directory.EnumerateDirectories(path);
            }
            return [];
        }

        public DirectoryInfo? Directory(string directory)
        {
            return TryCombine(directory, out var path) && System.IO.Directory.Exists(path) ? new DirectoryInfo(path) : null;
        }

        public bool Exist(string fileName)
        {
            return TryCombine(fileName, out var path) && System.IO.File.Exists(path);
        }

        public FileInfo? File(string fileName)
        {
            return TryCombine(fileName, out var path) && System.IO.File.Exists(path) ? new FileInfo(path) : null;
        }

        public IEnumerable<string> Files()
        {
            return System.IO.Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories);
        }

        public IEnumerable<string> Files(string directory)
        {
            return Files(directory, "*.*");
        }

        public IEnumerable<string> Files(string directory, string pattern)
        {
            if(TryCombine(directory, out var path) && System.IO.Directory.Exists(path))
            {
                return System.IO.Directory.EnumerateFiles(path, pattern, SearchOption.AllDirectories);
            }
            return [];
        }

        public Stream? OpenRead(string fileName)
        {
            if (TryCombine(fileName, out var path) && System.IO.File.Exists(path))
            {
                return System.IO.File.OpenRead(path);
            }
            return null;
        }

        public Stream? Create(string fileName)
        {
            if (TryCombine(fileName, out var path))
            {
                return System.IO.File.Create(path);
            }
            return null;
        }
    }
}
