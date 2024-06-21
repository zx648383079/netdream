using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.MessageService.Forms
{
    public class DebugForm
    {
        public string Target { get; set; } = string.Empty;
        public string Title { get; set; } = "Debug";
        public string Content { get; set; } = "Debug";

        public byte Type { get; set; }
    }
}
