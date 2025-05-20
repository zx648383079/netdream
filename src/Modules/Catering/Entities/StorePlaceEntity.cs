using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class StorePlaceEntity : IIdEntity
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int FloorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UserId { get; set; }
    
}