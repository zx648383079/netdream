using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Auth.Forms;
using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserAccount.Models;
using System.Threading.Tasks;

namespace NetDream.Api.Controllers.Auth
{
    [ApiController]
    [Route("open/[controller]")]
    public class AuthController(AuthRepository repository) : JsonController
    {

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(UserProfileModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public async Task<IActionResult> LoginAsync([FromBody] SignInForm form)
        {
            if (!string.IsNullOrEmpty(form.Password))
            {
                form.Password = JsonResponse.Decoder.Decrypt(form.Password);
            }
            var res = await repository.LoginAsync(form.GetContext());
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message, res.FailureReason);
            }
            return Render(res.Result);
        }


        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(UserProfileModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public async Task<IActionResult> Register([FromBody] SignUpForm form)
        {
            if (!string.IsNullOrEmpty(form.Password))
            {
                form.Password = JsonResponse.Decoder.Decrypt(form.Password);
            }
            var res = await repository.RegisterAsync(form.GetContext());
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message, res.FailureReason);
            }
            return Render(res.Result);
        }
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public async Task<IActionResult> Logout()
        {
            await repository.LogoutAsync();
            return RenderData(true);
        }

    }
}
