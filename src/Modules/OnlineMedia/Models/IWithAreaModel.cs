using NetDream.Shared.Models;

namespace NetDream.Modules.OnlineMedia.Models
{
    internal interface IWithAreaModel
    {
        public int AreaId { get; }

        public ListLabelItem? Area { set; }
    }
}