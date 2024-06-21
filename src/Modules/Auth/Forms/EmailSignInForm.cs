using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Forms
{
    public class EmailSignInForm: ISignInForm
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

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
