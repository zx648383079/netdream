using NetDream.Shared.Models;

namespace NetDream.Modules.OpenPlatform.Models
{
    public interface IWithPlatformModel
    {
        public int PlatformId { get; }

        public ListLabelItem? Platform { set; }
    }
}