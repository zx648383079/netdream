using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class AttributeListItem : AttributeEntity
    {
        public ListLabelItem? Group { get; set; }
    }
}
