using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
