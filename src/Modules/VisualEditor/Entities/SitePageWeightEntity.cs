using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.VisualEditor.Entities;
public class SitePageWeightEntity : IIdEntity
{
    public int Id { get; set; }
    public int PageId { get; set; }
    public int SiteId { get; set; }
    public int WeightId { get; set; }
    public int ParentId { get; set; }
    public byte ParentIndex { get; set; }
    public int Position { get; set; }
    
}