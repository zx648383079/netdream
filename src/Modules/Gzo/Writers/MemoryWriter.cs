using System.Collections.Generic;

namespace NetDream.Modules.Gzo.Writers
{
    public class MemoryWriter : List<MemoryFileItem>, IWriter
    {
        public string Mkdir(string folder)
        {
            return folder;
        }

        public void Write(string file, string content)
        {
            Add(new MemoryFileItem(file, content));
        }
    }
    public class MemoryFileItem(string path, string content)
    {
        public string Path { get; private set; } = path;

        public string Content { get; private set; } = content;
    }
}
