using NetDream.Modules.Forum.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Forum.Models
{
    public class ThreadModel: ThreadEntity, IWithUserModel
    {
        public string Content { get; set; } = string.Empty;
        public ThreadPostEntity? LastPost { get; set; }
        public bool IsNew { get; set; }
        public IUser? User { get; set; }
        public ForumEntity? Forum { get; set; }
        public ForumClassifyEntity? Classify { get; set; }

 
    }
}
