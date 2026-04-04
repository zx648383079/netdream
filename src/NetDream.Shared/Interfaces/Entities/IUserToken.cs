namespace NetDream.Shared.Interfaces
{
    public interface IUserToken : IUser
    {
        public string Token { get; set; }
    }
}
