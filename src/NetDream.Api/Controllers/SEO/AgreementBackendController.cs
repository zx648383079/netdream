using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.SEO.Entities;
using NetDream.Modules.SEO.Forms;
using NetDream.Modules.SEO.Models;
using NetDream.Modules.SEO.Repositories;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.SEO
{
    [Route("open/seo/admin/agreement")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class AgreementBackendController(AgreementRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<AgreementListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AgreementModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.Detail(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(AgreementEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] AgreementForm form)
        {
            var res = repository.Save(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
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
    }
}
