using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.MessageService.Forms
{
    public class MailProtocolSetting: IProtocolSetting
    {
        public string Host { get; set; } = string.Empty;

        public int Port { get; set; }

        public string Name { get; set; } = string.Empty;

        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
