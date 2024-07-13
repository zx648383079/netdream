using Modules.OnlineService.Entities;
using NetDream.Shared.Interfaces.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.OnlineService.Models
{
    public class MessageModel: MessageEntity, IWithUserModel
    {
        [Ignore]
        public IUser? User { get; set; }
    }
}
