using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.MessageService.Forms
{
    public class SmsProtocolSetting: IProtocolSetting
    {
        public string Protocol { get; set; } = string.Empty;

        
        public string AppKey { get; set; } = string.Empty;

        public string Secret { get; set; } = string.Empty;

        public string SignName { get; set; } = string.Empty;
    }
}
