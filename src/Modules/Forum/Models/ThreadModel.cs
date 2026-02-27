using NetDream.Modules.Forum.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Forum.Models
{
    public class ThreadModel: ThreadEntity, IWithUserModel, IWithForumModel, IWithClassifyModel
    {
        public string Content { get; set; } = string.Empty;
        public ThreadPostEntity? LastPost { get; set; }
        public bool IsNew { get; set; }
        public IUser? User { get; set; }
        public ForumLabelItem? Forum { get; set; }
        public ForumClassifyEntity? Classify { get; set; }

        public byte LikeType { get; set; }

        public bool IsCollected { get; set; }

        public bool IsReward { get; set; }
        public bool Digestable { get; internal set; }
        public bool Highlightable { get; internal set; }
        public bool Closeable { get; internal set; }
        public bool Topable { get; internal set; }
        public bool Editable { get; internal set; }
        public int RewardCount { get; internal set; }
        public ThreadLogModel[] RewardItems { get; internal set; }
    }
}
