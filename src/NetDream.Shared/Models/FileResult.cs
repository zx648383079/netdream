using NetDream.Shared.Interfaces;
using System.IO;

namespace NetDream.Shared.Models
{
    public class FileResult : IDownloadFile
    {
        public string Name { get; set; }

        public string FullPath { get; set; }
        public string FileType { get; set; } = "application/force-download";

        public FileResult(string fullPath)
            : this (Path.GetFileName(fullPath), fullPath)
        {
            
        }

        public FileResult(string name, string fullPath)
        {
            Name = name;
            FullPath = fullPath;
        }

        public Stream OpenRead()
        {
            return File.OpenRead(FullPath);
        }

    }
}
