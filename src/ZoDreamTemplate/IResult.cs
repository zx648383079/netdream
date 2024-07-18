using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.ZoDreamTemplate
{
    public interface IResult
    {
        public void Render(TextWriter writer, TemplateContext context);
    }
}
