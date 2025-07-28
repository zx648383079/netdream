using NetDream.Modules.Forum.Entities;
using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Forum.Models
{
    public class ForumModel: ForumEntity
    {

        public IList<ForumClassifyEntity>? Classifies { get; set; }

        public IList<ModeratorListItem>? Moderators { get; set; }
        public ForumListItem[] Children { get; internal set; }
        public ThreadModel[] ThreadTop { get; internal set; }
    }

    public class ModeratorListItem : ForumModeratorEntity, IWithUserModel
    {

        public IUser User { get; set; }
    }
}
