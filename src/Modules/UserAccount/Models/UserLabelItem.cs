using NetDream.Shared.Interfaces;

namespace NetDream.Modules.UserAccount.Models
{
    public class UserLabelItem : IUserSource
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public bool IsOnline { get; set; } = false;
    }
}
