using Modules.Forum.Entities;
using NetDream.Shared.Interfaces.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Forum.Models
{
    public class ThreadModel: ThreadEntity, IWithUserModel
    {
        [Ignore]
        public string Content { get; set; } = string.Empty;
        [Ignore]
        public ThreadPostEntity? LastPost { get; set; }
        [Ignore]
        public bool IsNew { get; set; }
        [Ignore]
        public IUser? User { get; set; }
        [Ignore]
        public ForumEntity? Forum { get; set; }
        [Ignore]
        public ForumClassifyEntity? Classify { get; set; }

 
    }
}
