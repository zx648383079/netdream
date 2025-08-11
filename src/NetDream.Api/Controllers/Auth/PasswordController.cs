using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Auth.Forms;
using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.OpenPlatform;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/[controller]")]
    [ApiController]
    public class PasswordController(AuthRepository repository) : JsonController
    {

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SendFindEmail(string email)
        {
            var res = repository.SendFindEmail(email);
            if (!res.Succeeded)
            {
                return RenderFailure(res);
            }
            return Render(new DataOneResponse<bool>(true)
            {
                Message = $"邮件已成功发送至 {email} 请注意查收！"
            });
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SendMobileCode(string mobile, string type = "login")
        {
            var res = repository.SendCode(new CodeRequestForm()
            {
                ToType = "mobile",
                To = mobile,
                Event = type
            });
            if (!res.Succeeded)
            {
                return RenderFailure(res);
            }
            return Render(new DataOneResponse<bool>(true)
            {
                Message = $"验证码已成功发送至 {mobile} 请注意查收！"
            });
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Reset([FromBody] ResetPasswordForm data)
        {
            var res = repository.ResetPassword(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res);
            }
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Update([FromBody] UpdatePasswordForm data, string old_password = "")
        {
            if (string.IsNullOrWhiteSpace(old_password))
            {
                data.VerifyType = "password";
                data.Verify = old_password;
            }
            var res = repository.UpdatePassword(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res);
            }
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SendCode([FromBody] CodeRequestForm data)
        {
            var res = repository.SendCode(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res);
            }
            return Render(new DataOneResponse<bool>(true)
            {
                Message = $"验证码已成功发送至 {data.To} 请注意查收！"
            });
        }
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult VerifyCode([FromBody] CodeVerifyForm data)
        {
            var res = repository.VerifyCode(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res);
            }
            return RenderData(true);
        }
    }
}
