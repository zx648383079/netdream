using Modules.Forum.Entities;
using NetDream.Shared.Interfaces.Entities;
using NPoco;

namespace NetDream.Modules.Forum.Models
{
    public class ThreadPostModel: ThreadPostEntity
    {
        [Ignore]
        public IUser? User { get; set; }
        [Ignore]
        public ThreadEntity? Thread { get; set; }
    }
}
