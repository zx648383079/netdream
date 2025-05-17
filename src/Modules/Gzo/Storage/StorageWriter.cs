using NetDream.Shared.Template;
using System;
using System.IO;
using System.Text;

namespace NetDream.Modules.Gzo.Storage
{
    public class LocalStorage : IStorage
    {
        public string CreateFolder(string folder)
        {
            if (!Directory.Exists(folder)) 
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }

        public ICodeWriter Create(string file)
        {
            return new CodeWriter(File.Create(file));
        }

        public void Write(string file, string content)
        {
            File.WriteAllText(file, content, new UTF8Encoding(false));
        }

        public bool Exists(string file)
        {
            return File.Exists(file);
        }
    }
}
