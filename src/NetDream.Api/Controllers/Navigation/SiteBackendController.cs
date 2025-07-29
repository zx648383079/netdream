using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Navigation.Entities;
using NetDream.Modules.Navigation.Forms;
using NetDream.Modules.Navigation.Models;
using NetDream.Modules.Navigation.Repositories;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.Navigation
{
    [Route("open/navigation/admin/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class SiteBackendController(SiteRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<SiteListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] SiteQueryForm form)
        {
            return RenderPage(repository.MangerList(form));
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(SiteModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.Get(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(SiteEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] SiteForm form)
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

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(SiteEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Check(string domain, int id = 0)
        {
            var res = repository.Check(domain, id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(SiteEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Scoring([FromBody] SiteScoringForm form)
        {
            var res = repository.Scoring(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ScoringLogListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult ScoreLog([FromQuery] ScoreQueryForm form)
        {
            return RenderPage(repository.GetScoreLog(form));
        }
    }
}
