using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using NetDream.Modules.Auth.Entities;
using NPoco;
using System;

namespace NetDream.Modules.Auth.Forms
{
    public class EmailSignUpForm: ISignUpForm
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string InviteCode { get; set; } = string.Empty;
        public string Agree { get; set; } = string.Empty;

        public string Account => Email;

        public IOperationResult<IUser> Verify(IDatabase db)
        {
            if (Validator.IsEmail(Email) || string.IsNullOrWhiteSpace(Password)) 
            {
                return OperationResult<IUser>.Fail(FailureReasons.ValidateError, new ArgumentNullException("email or password is empty"));
            }
            var user = db.Single<UserEntity>("where email=@0", Email) ?? throw new ArgumentException("email is not sign in");
            if (!BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                return OperationResult<IUser>.Fail(FailureReasons.ValidateError, new ArgumentException("password is error"));
            }
            return OperationResult<IUser>.Ok(user);
        }
    }
}
