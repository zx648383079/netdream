using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class StorePatronEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int UserId { get; set; }
    public int GroupId { get; set; }
    public int Amount { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}