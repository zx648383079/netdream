using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Auth.Models
{
    public class SignInResult : OperationResult<IUser>
    {
        public bool IsLockedOut { get; private set; }
        public bool IsNotAllowed { get; private set; }
        public bool RequiresTwoFactor { get; private set; }

        public SignInResult(IUser user) : base(user)
        {
            
        }

        public SignInResult(string message): base(0, message)
        {
            
        }
    }
}
