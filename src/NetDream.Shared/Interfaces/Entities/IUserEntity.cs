namespace NetDream.Shared.Interfaces.Entities
{
    public interface IWithUserModel
    {
        public int UserId { get; }

        public IUser? User { set; }
    }
}
