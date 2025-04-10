﻿using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Auth.Models
{
    public class UserListItem : IUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public bool IsOnline { get; set; } = false;
    }
}
