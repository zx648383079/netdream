using System.IO;

namespace NetDream.Shared.Models
{
    public class FileResult
    {
        public string Name { get; set; }

        public string FullPath { get; set; }

        public FileResult(string fullPath)
            : this (Path.GetFileName(fullPath), fullPath)
        {
            
        }

        public FileResult(string name, string fullPath)
        {
            Name = name;
            FullPath = fullPath;
        }
    }
}
