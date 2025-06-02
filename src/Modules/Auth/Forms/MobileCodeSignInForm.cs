using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Auth.Forms
{
    public class MobileCodeSignInForm: ISignInForm, IContextForm
    {
        [Required]
        public string Mobile { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
        public bool Agree { get; set; }

        public string Account => Mobile;

        public IOperationResult<IUserProfile> Verify(IContextRepository db)
        {
            if (!Agree)
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "Agreement is not confirm");
            }
            if (string.IsNullOrWhiteSpace(Mobile) || string.IsNullOrWhiteSpace(Code))
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "mobile or password is empty");
            }
            if (db.VerifyCode(Mobile, Code))
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "Code is error");
            }
            var res = db.Find(i => i.Mobile == Mobile);
            if (res.Succeeded)
            {
                return res;
            }
            return db.Create(new UserEntity()
            {
                Mobile = Mobile,
            });
        }
    }
}
