using NetDream.Shared.Models;

namespace NetDream.Modules.TradeTracker.Models
{
    public interface IWithProjectModel
    {
        public int ProjectId { get; }
        public ListLabelItem? Project { set; }
    }
}