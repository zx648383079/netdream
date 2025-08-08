using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.OpenPlatform.Repositories;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/admin/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class SettingBackendController(OpenRepository repository, IClientContext client) : JsonController
    {
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ListLabelItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Platform()
        {
            return RenderData(repository.GetByUser(client.UserId));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<IDictionary<string, IDictionary<string, string>>>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Option(int platform)
        {
            var data = new Dictionary<string, IDictionary<string, string>>();
            return RenderData(repository.OptionGet(client.UserId, platform, data));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult OptionSave(int platform, IDictionary<string, IDictionary<string, string>> option)
        {
            var res = repository.OptionSave(client.UserId, platform, option);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

    }
}
