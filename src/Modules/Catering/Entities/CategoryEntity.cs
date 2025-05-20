using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class CategoryEntity : IIdEntity
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public byte Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ParentId { get; set; }
    
}