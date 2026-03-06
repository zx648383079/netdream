using NetDream.Modules.Team.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Team.Models
{
    public class TeamUserListItem: IWithUserModel, IUser
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public int UserId => Id;
        public string Name { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public IUser? User { get; set; }

        public string Avatar => User?.Avatar ?? string.Empty;

        public bool IsOnline => User is IUserSource u && u.IsOnline;

        public TeamUserListItem()
        {
            
        }

        public TeamUserListItem(TeamUserEntity model)
        {
            Id = model.UserId;
            Name = model.Name;
            RoleId = model.RoleId;
        }
    }
}
