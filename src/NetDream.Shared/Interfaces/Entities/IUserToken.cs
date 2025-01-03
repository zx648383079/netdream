namespace NetDream.Shared.Interfaces.Entities
{
    public interface IUserToken : IUser
    {
        public string Token { get; set; }
    }
}
