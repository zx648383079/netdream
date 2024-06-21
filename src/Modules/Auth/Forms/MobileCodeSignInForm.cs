using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Modules.Auth.Entities;
using NPoco;
using NetDream.Shared.Models;

namespace NetDream.Modules.Auth.Forms
{
    public class MobileCodeSignInForm: ISignInForm
    {
        public string Mobile { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Account => Mobile;

        public IOperationResult<IUser> Verify(IDatabase db)
        {
            if (string.IsNullOrWhiteSpace(Mobile) || string.IsNullOrWhiteSpace(Code))
            {
                throw new ArgumentNullException("mobile or password is empty");
            }
            var user = db.Single<UserEntity>("where mobile=@0", Mobile) ?? throw new ArgumentException("email is not sign in");
            // TODO 验证
            return OperationResult<IUser>.Ok(user);
        }
    }
}
