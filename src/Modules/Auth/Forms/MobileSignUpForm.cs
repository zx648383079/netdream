using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Auth.Forms
{
    public class MobileSignUpForm: ISignUpForm, IContextForm
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Mobile { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
        [Compare(nameof(ConfirmPassword))]
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string InviteCode { get; set; } = string.Empty;
        public bool Agree { get; set; }

        public string Account => Mobile;

        public bool Agreement => Agree;

        public IOperationResult<IUserProfile> Verify(IContextRepository db)
        {
            if (!Agree)
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "Agreement is not confirm");
            }
            if (Shared.Helpers.Validator.IsMobile(Mobile) || string.IsNullOrWhiteSpace(Code) || string.IsNullOrWhiteSpace(Password))
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "mobile or password is empty");
            }
            if (Password != ConfirmPassword)
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "password is not confirm");
            }
            if (db.VerifyCode(Mobile, Code))
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "Code is error");
            }
            if (db.Exists(i => i.Mobile == Mobile))
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "email is sign in");
            }
            return db.Create(new UserEntity()
            {
                Name = Name,
                Mobile = Mobile,
                Password = BCrypt.Net.BCrypt.HashPassword(Password),
            }, InviteCode);
        }
    }
}
