using NetDream.Shared.Models;

namespace NetDream.Modules.TradeTracker.Forms
{
    public class LogQueryForm : QueryForm
    {
        public int Product { get; set; }

        public int Channel { get; set; }

        public int Type { get; set; }
    }
}
