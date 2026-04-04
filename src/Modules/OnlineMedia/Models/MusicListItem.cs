using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.OnlineMedia.Models
{
    public class MusicListItem : MusicListEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
