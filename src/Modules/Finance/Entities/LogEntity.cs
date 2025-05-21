using NetDream.Shared.Interfaces.Entities;
using System;

namespace NetDream.Modules.Finance.Entities;
public class LogEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public byte Type { get; set; }
    public float Money { get; set; }
    public float FrozenMoney { get; set; }
    public int AccountId { get; set; }
    public int ChannelId { get; set; }
    public int ProjectId { get; set; }
    public int BudgetId { get; set; }
    public string Remark { get; set; } = string.Empty;
    public DateTime HappenedAt { get; set; }
    public string OutTradeNo { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string TradingObject { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}