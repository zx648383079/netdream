using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Counter.Forms;
using NetDream.Modules.Counter.Models;
using NetDream.Modules.Counter.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.Counter
{
    [Route("open/counter/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class VisitController(StateRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ITrendAnalysis>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] AnalysisQueryForm form)
        {
            return RenderPage(repository.AllUrl(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ITrendAnalysis>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Enter([FromQuery] AnalysisQueryForm form)
        {
            return RenderPage(repository.EnterUrl(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ITrendAnalysis>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Domain([FromQuery] AnalysisQueryForm form)
        {
            return RenderData(repository.Domain(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ITrendAnalysis>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Jump([FromQuery] AnalysisQueryForm form)
        {
            return RenderPage(repository.Jump(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ITrendAnalysis>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult District([FromQuery] AnalysisQueryForm form)
        {
            return RenderPage(repository.Jump(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ITrendAnalysis>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Client([FromQuery] AnalysisQueryForm form)
        {
            return RenderPage(repository.Jump(form));
        }
    }
}
