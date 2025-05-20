using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Legwork.Entities;
public class OrderLogEntity : IIdEntity, ICreatedEntity
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public byte Status { get; set; }
    public string Remark { get; set; } = string.Empty;
    public int CreatedAt { get; set; }
    
}