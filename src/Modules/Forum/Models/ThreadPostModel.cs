using NetDream.Modules.Forum.Entities;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Forum.Models
{
    public class ThreadPostModel: ThreadPostEntity, IWithUserModel, IWithThreadModel
    {
        public IUser? User { get; set; }
        public ListArticleItem? Thread { get; set; }
    }
}
