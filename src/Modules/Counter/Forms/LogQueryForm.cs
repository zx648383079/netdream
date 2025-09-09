using NetDream.Shared.Models;

namespace NetDream.Modules.Counter.Forms
{
    public class LogQueryForm : QueryForm
    {
        public string StartAt { get; set; }
        public string EndAt { get; set; }

        public string Ip { get; set; }
        public string Pathname { get; set; }
        public string Hostname { get; set; }
        public string UserAgent { get; set; }
        /// <summary>
        /// 根据时间跳转
        /// </summary>
        public string Goto { get; set; }
    }
}
