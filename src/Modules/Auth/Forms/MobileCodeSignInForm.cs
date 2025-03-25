using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Models;
using System;
using System.Linq;
using NetDream.Modules.Auth.Models;

namespace NetDream.Modules.Auth.Forms
{
    public class MobileCodeSignInForm: ISignInForm, IContextForm
    {
        public string Mobile { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Account => Mobile;

        public IOperationResult<IUser> Verify(AuthContext db)
        {
            if (string.IsNullOrWhiteSpace(Mobile) || string.IsNullOrWhiteSpace(Code))
            {
                return OperationResult<IUser>.Fail("mobile or password is empty");
            }
            var user = db.Users.Where(i => i.Mobile == Mobile).SingleOrDefault() ?? throw new ArgumentException("email is not sign in");
            // TODO 验证
            return OperationResult<IUser>.Ok(new UserModel(user));
        }
    }
}
