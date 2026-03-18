using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Wallet.Entities
{
    
    public class PaymentEntity : IIdEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Settings { get; set; } = string.Empty;
    }
}
