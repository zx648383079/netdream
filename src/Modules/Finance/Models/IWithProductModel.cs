using NetDream.Shared.Models;

namespace NetDream.Modules.Finance.Models
{
    public interface IWithProductModel
    {
        public int ProductId { get; }

        public ListLabelItem Product { set; }
    }
}
