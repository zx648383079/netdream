using NetDream.Modules.Forum.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Forum.Models
{
    public class ThreadLogModel: ThreadLogEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
