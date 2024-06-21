using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.MessageService.Models
{
    public class MessageSetting
    {
        /// <summary>
        /// 发送间隔
        /// </summary>
        public int Space { get; set; } = 120;
        /// <summary>
        /// 每个ip一天数量
        /// </summary>
        public int Everyone { get; set; } = 20;
        /// <summary>
        /// 每天发送数量
        /// </summary>
        public int Everyday { get; set; } = 2000;
    }
}
