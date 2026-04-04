using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Wallet.Entities
{
    /// <summary>
    /// 记录账户资金变化的，不可更改
    /// </summary>
    public class WalletLogEntity : IIdEntity, ICreatedEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Money { get; set; }

        public int Credits { get; set; }

        public string Hash { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
