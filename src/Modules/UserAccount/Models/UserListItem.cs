using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.UserAccount.Models
{
    public class UserListItem : IUserSource
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public bool IsOnline { get; set; } = false;
    }
}
