using NetDream.Shared.Helpers;
using NetDream.Shared.Models;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class OrderQueryForm : QueryForm
    {
        public string SeriesNumber { get; set; } = string.Empty;
        public byte Status { get; set; }
        public int LogId { get; set; }
        public string StartAt { get; set; } = string.Empty;
        public string EndAt { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Conginee { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public int StartAtTime => TimeHelper.TimestampFrom(StartAt);
        public int EndAtTime => TimeHelper.TimestampFrom(EndAt);
    }
}
