using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineMedia.Models
{
    public class MusicListModel : MusicListEntity, IWithUserModel
    {
        public IUser? User { get; set; }

        public MusicEntity[] Items {get; set;}
    }
}
