using NetDream.Modules.Auth.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NetDream.Modules.Auth.Forms
{
    public class EmailSignInForm: ISignInForm, IContextForm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        public string Account => Email;

        public IOperationResult<IUser> Verify(AuthContext db)
        {
            if (!Shared.Helpers.Validator.IsEmail(Email) || string.IsNullOrWhiteSpace(Password)) 
            {
                return OperationResult<IUser>.Fail(FailureReasons.ValidateError, "email or password is empty");
            }
            var user = db.Users.Where(i => i.Email == Email).SingleOrDefault();
            if (user is null)
            {
                return OperationResult<IUser>.Fail(FailureReasons.ValidateError, "email is not sign in");
            }
            if (!BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                return OperationResult<IUser>.Fail(FailureReasons.ValidateError, new ArgumentException("password is error"));
            }
            return OperationResult<IUser>.Ok(new UserModel(user)
            {
                IsOnline = true,
            });
        }
    }
}
