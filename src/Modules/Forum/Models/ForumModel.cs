using NetDream.Modules.Forum.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Forum.Models
{
    public class ForumModel: ForumEntity
    {

        public IList<ForumClassifyEntity>? Classifies { get; set; }

        public IList<ForumModeratorEntity>? Moderators { get; set; }
    }
}
