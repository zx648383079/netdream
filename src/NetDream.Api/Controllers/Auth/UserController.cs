using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserAccount.Forms;
using NetDream.Modules.UserAccount.Models;
using NetDream.Modules.UserAccount.Repositories;
using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/[controller]")]
    [ApiController]
    public class UserController(UserRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(string extra = "")
        {
            return Render(repository.GetCurrentProfile(extra));
        }

        
        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataResponse<StatisticsItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Statistics()
        {
            return RenderData(repository.Statistics());
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Avatar(IFormFile file)
        {
            var res = repository.ChangeAvatar(new FormUploadFile(file));
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Update([FromBody] ProfileUpdateForm data)
        {
            var res = repository.UpdateProfile(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult UpdateAccount([FromBody] AccountUpdateForm data)
        {
            var res = repository.UpdateAccount(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(EmailLabelItem), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Check(string email)
        {
            var res = repository.CheckEmail(email);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataResponse<StatisticsItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Role()
        {
            return RenderData(repository.Statistics());
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataResponse<StatisticsItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Setting()
        {
            return RenderData(repository.SettingGet());
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SettingSave([FromBody] Dictionary<string, string> data)
        {
            var res = repository.SettingSave(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(UserProfileModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Ticket(string email)
        {
            var res = repository.CheckEmail(email);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(res.Result);
        }
    }
}
