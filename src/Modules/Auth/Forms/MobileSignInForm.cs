using NetDream.Core.Interfaces.Entities;
using NetDream.Core.Interfaces;
using NetDream.Core.Interfaces.Forms;
using NetDream.Modules.Auth.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDream.Core.Models;

namespace NetDream.Modules.Auth.Forms
{
    public class MobileSignInForm: ISignInForm
    {
        public string Mobile { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Account => Mobile;

        public IOperationResult<IUser> Verify(IDatabase db)
        {
            if (string.IsNullOrWhiteSpace(Mobile) || string.IsNullOrWhiteSpace(Password))
            {
                throw new ArgumentNullException("mobile or password is empty");
            }
            var user = db.Single<UserEntity>("where mobile=@0", Mobile) ?? throw new ArgumentException("email is not sign in");
            if (!BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                throw new ArgumentException("password is error");
            }
            return OperationResult<IUser>.Ok(user);
        }
    }
}
