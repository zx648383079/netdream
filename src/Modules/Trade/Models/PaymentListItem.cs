using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Trade.Models
{
    public class PaymentListItem : ICodeOptionItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
