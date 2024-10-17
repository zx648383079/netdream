using NetDream.Modules.Chat.Entities;
using NetDream.Shared.Interfaces.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Chat.Models
{
    public class FriendModel: FriendEntity, IWithUserModel
    {
        [Ignore]
        public IUser? User { get; set; }
    }
}
