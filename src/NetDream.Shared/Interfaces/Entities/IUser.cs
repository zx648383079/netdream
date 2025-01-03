﻿namespace NetDream.Shared.Interfaces.Entities
{
    public interface IUser
    {
        public int Id { get; }

        public string Name { get; }

        public string Avatar { get; }

        public bool IsOnline { get; }
    }
}
