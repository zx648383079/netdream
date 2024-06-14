using NetDream.Core.Interfaces.Entities;
using NetDream.Core.Interfaces;
using NetDream.Core.Interfaces.Forms;
using NetDream.Modules.Auth.Entities;
using NPoco;
using NetDream.Core.Models;

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
