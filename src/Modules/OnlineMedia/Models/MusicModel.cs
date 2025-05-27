using NetDream.Modules.OnlineMedia.Entities;

namespace NetDream.Modules.OnlineMedia.Models
{
    public class MusicModel : MusicEntity
    {
        public MusicFileEntity[]? Files { get; set; }
    }
}
