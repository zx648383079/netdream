using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.AdSense.Forms
{
    public class AdForm
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PositionId { get; set; }
        public byte Type { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int StartAt { get; set; }
        public int EndAt { get; set; }
        public byte Status { get; set; }
    }
}
