using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Comment.Models
{
    public class GuestUser: IUser
    {
        public string Name { get; internal set; }
        public string Email { get; internal set; }
        public string Url { get; internal set; }

        public int Id => 0;

        public string Avatar => string.Empty;
    }
}
