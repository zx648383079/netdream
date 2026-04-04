using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Wallet.Entities
{
    public class WalletEntity : IIdEntity, ITimestampEntity
    {
        public int Id {  get; set; }

        public int Money { get; set; }
        public int Credits { get; set; }

        public string Password { get; set; } = string.Empty;

        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
    }
}
