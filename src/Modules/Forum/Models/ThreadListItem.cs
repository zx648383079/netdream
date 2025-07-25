using NetDream.Modules.Forum.Entities;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Forum.Models
{
    public class ThreadListItem : ThreadEntity, IWithUserModel, IWithForumModel, IWithClassifyModel
    {
        public int AgreeCount { get; set; }

        public int DisagreeCount { get; set; }
        public string Brief { get; set; } = string.Empty;
        public IUser[] UserItems { get; set; } = [];
        public LinkLabelItem[] ImageItems { get; set; } = [];
        public bool IsNew { get; set; }
        public IUser? User { get; set; }
        public ForumLabelItem? Forum { get; set; }
        public ForumClassifyEntity? Classify { get; set; }
    }
}
