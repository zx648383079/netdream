using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Models
{
    public class UserEquityCardModel: UserEquityCardEntity
    {
        public EquityCardEntity? Card { get; set; }
    }
}
