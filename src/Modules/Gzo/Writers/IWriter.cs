using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Gzo.Writers
{
    public interface IWriter
    {
        public string Mkdir(string folder);

        public void Write(string file, string content);
    }
}
