using NetDream.Shared.Interfaces;
using System.IO;

namespace NetDream.Modules.Storage.Models
{
    public class FileOutputResult : IDownloadFile
    {
        public string Name { get; set; }

        public string FullPath { get; set; }
        public string FileType { get; set; } = "application/force-download";

        public FileOutputResult(string fullPath)
            : this (Path.GetFileName(fullPath), fullPath)
        {
            
        }

        public FileOutputResult(string name, string fullPath)
        {
            Name = name;
            FullPath = fullPath;
        }

        public FileOutputResult(FileInfo fileInfo)
            : this(fileInfo.Name, fileInfo.FullName)
        {
            
        }

        public Stream OpenRead()
        {
            return File.OpenRead(FullPath);
        }

    }
}
