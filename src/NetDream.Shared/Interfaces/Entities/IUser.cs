using System;

namespace NetDream.Shared.Interfaces.Entities
{
    public interface IUser
    {
        public int Id { get; }

        public string Name { get; }

        public string Avatar { get; }
    }
    /// <summary>
    /// 用于其他引用
    /// </summary>
    public interface IUserSource : IUser
    {
        public bool IsOnline { get; }
    }
    /// <summary>
    /// 用于内部当前用户
    /// </summary>
    public interface IUserProfile : IUser
    {
        public string Email { get; }
        public string Mobile { get; }

        public int Sex { get; }
        public DateOnly Birthday { get; }

        public int ParentId { get; }

        public int CreatedAt { get; }
    }
}
