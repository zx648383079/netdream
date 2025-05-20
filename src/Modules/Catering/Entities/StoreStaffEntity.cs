using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class StoreStaffEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}