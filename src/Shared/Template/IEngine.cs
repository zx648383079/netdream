using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Template
{
    public interface IEngine
    {

        public ITemplate LoadTemplate(string path);
    }
}
