using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.ZoDreamTemplate
{
    public interface ITemplate
    {

        public void Render(TextWriter writer);
    }
}
