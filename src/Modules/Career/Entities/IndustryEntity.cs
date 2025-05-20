using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class IndustryEntity : IIdEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
}