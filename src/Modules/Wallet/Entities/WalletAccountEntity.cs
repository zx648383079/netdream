using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Wallet.Entities
{
    /// <summary>
    /// 第三方电子账户，包括银行卡等
    /// </summary>
    public class WalletAccountEntity : IIdEntity, ITimestampEntity
    {
        public int Id {  get; set; }

        public byte Type { get; set; }

        public string Account { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int ExpiredAt { get; set; }
        public byte Status { get; set; }
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
    }
}
