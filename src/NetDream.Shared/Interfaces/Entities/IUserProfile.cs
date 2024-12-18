namespace NetDream.Shared.Interfaces.Entities
{
    public interface IUserProfile : IUser
    {
        public int BulletinCount { get; }
    }
}
