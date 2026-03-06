using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Chat.Models
{
    public class UserProfileModel(IUser user)
    {
        public string Name => user.Name;

        public IUser User => user;

        public string Signature { get; set; } = string.Empty;
        public int NewCount { get; set; }


    }
}
