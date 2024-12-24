using NetDream.Modules.Forum.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Forum.Models
{
    public class ThreadPostModel: ThreadPostEntity
    {
        public IUser? User { get; set; }
        public ThreadEntity? Thread { get; set; }
    }
}
