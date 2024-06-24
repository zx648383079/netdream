using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Shared.Helpers
{
    public static class FileHelper
    {
        public static string Combine(params string[] files)
        {
            return Path.GetFullPath(Path.Combine(files));
        }

        public static string RepairSeparator(string path)
        {
            return path.Replace('\\', '/');
        }

        public static string FilterPath(string path, bool isRoot = true)
        {
            if (isRoot)
            {
                return Path.GetFullPath(path);
            }
            var args = new List<string>();
            var baseFile = RepairSeparator(path);
            foreach (var item in baseFile.Split('/'))
            {
                if (item == "." || item == "..")
                {
                    continue;
                }
                if (!string.IsNullOrWhiteSpace(item))
                {
                    args.Add(item);
                }
            }
            return string.Join('/', args);
        }

        public static void EachFile(string folder,
            Action<FileInfo> success,
            CancellationToken token = default)
        {
            EachFile(new DirectoryInfo(folder), success, token);
        }

        public static void EachFile(DirectoryInfo folder,
            Action<FileInfo> success,
            CancellationToken token = default)
        {
            try
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                foreach (var item in folder.EnumerateDirectories())
                {
                    EachFile(item, success, token);
                }
                foreach (var item in folder.EnumerateFiles())
                {
                    success.Invoke(item);
                }
            }
            catch (UnauthorizedAccessException)
            {

            }
        }
    }
}
