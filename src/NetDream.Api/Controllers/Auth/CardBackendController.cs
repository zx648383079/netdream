using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Entities;
using NetDream.Modules.UserIdentity.Forms;
using NetDream.Modules.UserIdentity.Models;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/admin/card")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class CardBackendController(CardRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<EquityCardListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(EquityCardEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] EquityCardForm form)
        {
            var res = repository.Save(form);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.Remove(id);
            return RenderData(true);
        }

        [HttpGet]
        [Route("user")]
        [ProducesResponseType(typeof(PageResponse<UserEquityCardEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult GetUser(int user, [FromBody] QueryForm form)
        {
            return RenderData(repository.UserCardList(user, form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(EquityCardEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult UserUpdate([FromBody] BindingCardForm data)
        {
            var res = repository.UserUpdate(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<EquityCardLabelItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Search([FromQuery] QueryForm form, int[] id)
        {
            return RenderPage(repository.Search(form, id));
        }
    }
}
