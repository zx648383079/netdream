using Modules.Forum.Entities;
using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Forum.Models
{
    public class ForumModel: ForumEntity
    {

        public IList<ForumClassifyEntity>? Classifies { get; set; }

        public IList<ForumModeratorEntity>? Moderators { get; set; }
    }
}
