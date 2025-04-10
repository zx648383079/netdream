﻿using NetDream.Modules.Chat.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Chat.Models
{
    public class FriendModel: FriendEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
