using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Plan.Entities;
public class ShareUserEntity : IIdEntity, ICreatedEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ShareId { get; set; }
    public int DeletedAt { get; set; }
    public int CreatedAt { get; set; }
    
}