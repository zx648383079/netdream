using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Legwork.Entities;
public class ServiceEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CatId { get; set; }
    public string Thumb { get; set; } = string.Empty;
    public string Brief { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Form { get; set; } = string.Empty;
    public byte Status { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}