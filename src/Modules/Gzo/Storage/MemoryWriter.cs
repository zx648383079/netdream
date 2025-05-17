using NetDream.Shared.Template;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDream.Modules.Gzo.Storage
{
    public class MemoryStorage : List<MemoryFileItem>, IStorage
    {
        public string CreateFolder(string folder)
        {
            return folder;
        }

        public bool Exists(string file)
        {
            return this.Where(i => i.Path == file).Any();
        }
        public ICodeWriter Create(string file)
        {
            var sb = new StringBuilder();
            Add(new MemoryFileItem(file, sb));
            return new CodeWriter(sb);
        }

        public void Write(string file, string content)
        {
            Add(new MemoryFileItem(file, new StringBuilder(content)));
        }
    }
    public class MemoryFileItem(string path, StringBuilder builder)
    {
        public string Path { get; private set; } = path;

        public string Content => builder.ToString();
    }
}
