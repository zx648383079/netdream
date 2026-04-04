using NetDream.Modules.OpenPlatform.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Modules.OpenPlatform.Models
{
    public class TokenListItem : UserTokenEntity, IWithUserModel, IWithPlatformModel
    {
        public IUser? User { get; set; }

        public ListLabelItem? Platform { get; set; }
    }
}
