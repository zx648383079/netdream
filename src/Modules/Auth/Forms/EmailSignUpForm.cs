using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.UserAccount;
using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NetDream.Modules.Auth.Forms
{
    public class EmailSignUpForm: ISignUpForm, IContextForm
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Compare(nameof(ConfirmPassword))]
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string InviteCode { get; set; } = string.Empty;
        public bool Agree { get; set; }

        public string Account => Email;

        public IOperationResult<IUserProfile> Verify(IContextRepository db)
        {
            if (!Agree)
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "Agreement is not confirm");
            }
            if (Shared.Helpers.Validator.IsEmail(Email) || string.IsNullOrWhiteSpace(Password)) 
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "email or password is empty");
            }
            if (Password != ConfirmPassword)
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "password is not confirm");
            }
            if (db.Exists(i => i.Email == Email))
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "email is sign in");
            }
            return db.Create(new UserEntity()
            {
                Name = Name,
                Email = Email,
                Password = BCrypt.Net.BCrypt.HashPassword(Password),
            });
        }
    }
}
