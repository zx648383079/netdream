using NetDream.Modules.Auth.Repositories;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Auth.Forms
{
    public class MobileSignInForm: ISignInForm, IContextForm
    {
        [Required]
        public string Mobile { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        public string Account => Mobile;

        public IOperationResult<IUserProfile> Verify(IContextRepository db)
        {
            if (string.IsNullOrWhiteSpace(Mobile) || string.IsNullOrWhiteSpace(Password))
            {
                return OperationResult<IUserProfile>.Fail("mobile or password is empty");
            }
            return db.Find(i => i.Mobile == Mobile, Password);
        }
    }
}
