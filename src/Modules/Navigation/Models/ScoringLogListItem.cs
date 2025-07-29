using NetDream.Modules.Navigation.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Navigation.Models
{
    public class ScoringLogListItem : SiteScoringLogEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
