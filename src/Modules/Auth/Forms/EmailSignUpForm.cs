using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using System;
using System.Linq;

namespace NetDream.Modules.Auth.Forms
{
    public class EmailSignUpForm: ISignUpForm, IContextForm
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string InviteCode { get; set; } = string.Empty;
        public bool Agree { get; set; }

        public string Account => Email;

        public IOperationResult<IUser> Verify(AuthContext db)
        {
            if (!Agree)
            {
                return OperationResult<IUser>.Fail(FailureReasons.ValidateError, "Agreement is not confirm");
            }
            if (Validator.IsEmail(Email) || string.IsNullOrWhiteSpace(Password)) 
            {
                return OperationResult<IUser>.Fail(FailureReasons.ValidateError, "email or password is empty");
            }
            if (Password != ConfirmPassword)
            {
                return OperationResult<IUser>.Fail(FailureReasons.ValidateError, "password is not confirm");
            }
            var isExist = db.Users.Where(i => i.Email == Email).Any();
            if (isExist)
            {
                return OperationResult<IUser>.Fail(FailureReasons.ValidateError, "email is sign in");
            }
            var user = new UserEntity()
            {
                Name = Name,
                Email = Email,

                Password = BCrypt.Net.BCrypt.HashPassword(Password),
            };
            return OperationResult<IUser>.Ok(user);
        }
    }
}
