using NetDream.Modules.Auth.Repositories;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using System.ComponentModel.DataAnnotations;

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

        public IOperationResult<IUserProfile> Verify(IContextRepository db)
        {
            if (!Shared.Helpers.Validator.IsEmail(Email) || string.IsNullOrWhiteSpace(Password)) 
            {
                return OperationResult<IUserProfile>.Fail(FailureReasons.ValidateError, "email or password is empty");
            }
            return db.Find(i => i.Email == Email, Password);
        }
    }
}
