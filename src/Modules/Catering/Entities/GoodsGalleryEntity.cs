using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Entities;
public class GoodsGalleryEntity : IIdEntity
{
    public int Id { get; set; }
    public int GoodsId { get; set; }
    public string Thumb { get; set; } = string.Empty;
    public byte FileType { get; set; }
    public string File { get; set; } = string.Empty;
    
}