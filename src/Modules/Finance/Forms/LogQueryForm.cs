using NetDream.Shared.Models;

namespace NetDream.Modules.Finance.Forms
{
    public class LogQueryForm : QueryForm
    {
        public int Type { get; set; }

        public int Account { get; set; }

        public int Budget { get; set; }

        public string StartAt { get; set; } = string.Empty;
        public string EndAt { get; set; } = string.Empty;
        public string Goto { get; set; } = string.Empty;
    }

    public class LogSearchForm : QueryBoundForm
    {
        public int Type { get; set; }
    }

}
