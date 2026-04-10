using NetDream.Shared.Interfaces;

namespace NetDream.Modules.OnlineMedia.Models
{
    internal interface IWithAreaModel
    {
        public int AreaId { get; }

        public IListLabelItem? Area { set; }
    }
}