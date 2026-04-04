using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Wallet.Entities
{
    /// <summary>
    /// 资金变化流水，可以变更，可能未实际入账
    /// </summary>
    public class AccountLogEntity  : IIdEntity, ICreatedEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public byte Type { get; set; }
        
        public int ItemId { get; set; }
        public int Money { get; set; }
        
        public int TotalMoney { get; set; }
        public int Credits { get; set; }
        public int TotalCredits { get; set; }
        public byte Status { get; set; }
        public string Remark { get; set; } = string.Empty;
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
