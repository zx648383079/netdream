using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Auth.Models
{
    public class UserModel : UserProfileModel, IUserToken
    {

        public string Token { get; set; } = string.Empty;
    }
}
