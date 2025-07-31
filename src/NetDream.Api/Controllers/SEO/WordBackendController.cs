using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.SEO.Entities;
using NetDream.Modules.SEO.Forms;
using NetDream.Modules.SEO.Repositories;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.SEO
{
    [Route("open/seo/admin/word")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class WordBackendController(WordRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<BlackWordEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(BlackWordEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] WordForm form)
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
