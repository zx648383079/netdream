using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Chat.Forms
{
    public class MessageForm
    {
        public int Type { get; set; }
        public string Content { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public LinkExtraRule[]? ExtraRule { get; set; }
    }
}
