using NetDream.Modules.Team.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Team.Models
{
    public class TeamModel: TeamEntity
    {
        public IPage<TeamUserListItem>? Users { get; set; }
    }
}
