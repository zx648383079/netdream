﻿using NetDream.Modules.Chat.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Chat.Models
{
    public class GroupUserModel: GroupUserEntity, IWithUserModel, IUser
    {
        public IUser? User { get; set; }

        public string Avatar => User?.Avatar ?? string.Empty;

        public bool IsOnline => User is IUserSource u && u.IsOnline;

        public GroupUserModel()
        {
            
        }

        public GroupUserModel(GroupUserEntity model)
        {
            Id = model.Id;
            Name = model.Name;
            UserId = model.UserId;
            RoleId = model.RoleId;
            Status = model.Status;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
        }
    }
}
