using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.MessageService.Forms;
using NetDream.Modules.MessageService.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Interfaces.Forms;

namespace NetDream.Api.Controllers.MessageService
{
    [Route("open/ms/admin/option")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class SettingController(MessageRepository repository) : JsonController
    {
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<IFormInput>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Mail()
        {
            return RenderData(repository.OptionForm(true));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult MailSave([FromBody] MailProtocolSetting data)
        {
            var res = repository.OptionSave(data, true);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<IFormInput>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Sms()
        {
            return RenderData(repository.OptionForm(false));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SmsSave([FromBody] SmsProtocolSetting data)
        {
            var res = repository.OptionSave(data, false);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }
    }
}
