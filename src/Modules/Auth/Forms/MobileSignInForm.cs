﻿using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Modules.Auth.Entities;
using System;
using NetDream.Shared.Models;
using System.Linq;

namespace NetDream.Modules.Auth.Forms
{
    public class MobileSignInForm: ISignInForm, IContextForm
    {
        public string Mobile { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Account => Mobile;

        public IOperationResult<IUser> Verify(AuthContext db)
        {
            if (string.IsNullOrWhiteSpace(Mobile) || string.IsNullOrWhiteSpace(Password))
            {
                throw new ArgumentNullException("mobile or password is empty");
            }
            var user = db.Users.Where(i => i.Mobile == Mobile).Single() ?? throw new ArgumentException("email is not sign in");
            if (!BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                throw new ArgumentException("password is error");
            }
            return OperationResult<IUser>.Ok(user);
        }
    }
}
