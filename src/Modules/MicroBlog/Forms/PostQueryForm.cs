using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.MicroBlog.Forms
{
    public class PostQueryForm : QueryForm
    {
        public int User { get; set; }

        public int Topic { get; set; }
        public int Id { get; set; }
    }
}
