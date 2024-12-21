using System.IO;
using System.Text;

namespace NetDream.Modules.Gzo.Writers
{
    public class StorageWriter : IWriter
    {
        public string Mkdir(string folder)
        {
            if (!Directory.Exists(folder)) 
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }

        public void Write(string file, string content)
        {
            File.WriteAllText(file, content, new UTF8Encoding(false));
        }
    }
}
