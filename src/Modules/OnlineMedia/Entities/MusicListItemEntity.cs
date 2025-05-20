using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineMedia.Entities;
public class MusicListItemEntity : IIdEntity
{
    public int Id { get; set; }
    public int ListId { get; set; }
    public int MusicId { get; set; }
    
}