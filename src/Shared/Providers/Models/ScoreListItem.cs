using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Models
{
    public class ScoreListItem : ScoreLogEntity, IWithUserModel
    {
        public IUser? User { get; set; }
    }
}
