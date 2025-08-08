using NetDream.Modules.UserIdentity.Entities;

namespace NetDream.Modules.UserIdentity.Models
{
    public class EquityCardListItem: EquityCardEntity
    {
        public int Amount { get; set; }
    }

    public class EquityCardLabelItem
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;
    }
}
