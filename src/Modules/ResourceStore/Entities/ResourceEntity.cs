using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.ResourceStore.Entities;
public class ResourceEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Keywords { get; set; } = string.Empty;
    public string Thumb { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Size { get; set; }
    public float Score { get; set; }
    public int UserId { get; set; }
    public byte PreviewType { get; set; }
    public int CatId { get; set; }
    public int Price { get; set; }
    public byte IsCommercial { get; set; }
    public byte IsReprint { get; set; }
    public int CommentCount { get; set; }
    public int ViewCount { get; set; }
    public int DownloadCount { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}