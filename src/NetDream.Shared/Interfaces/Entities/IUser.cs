using System;

namespace NetDream.Shared.Interfaces.Entities
{
    public interface IUser
    {
        public int Id { get; }

        public string Name { get; }

        public string Avatar { get; }

        public bool IsOnline { get; }
    }

    public interface IUserProfile
    {
        public int Id { get; }

        public string Name { get; }
        public string Email { get; }
        public string Mobile { get; }

        public string Avatar { get; }

        public int Sex { get; }
        public DateOnly Birthday { get; }

        public int ParentId { get; }

        public int CreatedAt { get; }
    }
}
