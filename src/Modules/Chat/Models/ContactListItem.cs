using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Chat.Models
{
    public class ContactListItem: IUserSource
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public IUser User { get; set; }

        public int Id => User.Id;

        public string Avatar => User.Avatar;

        public bool IsOnline => User is IUserSource u && u.IsOnline;
    }
}
