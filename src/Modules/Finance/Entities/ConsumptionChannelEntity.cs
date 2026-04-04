using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Finance.Entities;
public class ConsumptionChannelEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}