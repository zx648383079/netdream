namespace NetDream.Shared.Interfaces
{
    public interface IWithUserModel
    {
        public int UserId { get; }

        public IUser? User { set; }
    }
}
