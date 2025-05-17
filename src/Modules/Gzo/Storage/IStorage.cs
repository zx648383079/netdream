using NetDream.Shared.Template;

namespace NetDream.Modules.Gzo.Storage
{
    public interface IStorage
    {
        public string CreateFolder(string folder);

        public bool Exists(string file);

        public ICodeWriter Create(string file);

        public void Write(string file, string content);
    }
}
