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
    public class ThreadLogModel: ThreadLogEntity, IWithUserModel
    {
        [Ignore]
        public IUser? User { get; set; }
    }
}
