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
    public class SourceController(StateRepository repository) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<ITrendAnalysis>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] AnalysisQueryForm form)
        {
            return RenderData(repository.AllSource(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ITrendAnalysis>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Engine([FromQuery] AnalysisQueryForm form)
        {
            return RenderData(repository.SearchEngine(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ITrendAnalysis>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SearchWord([FromQuery] AnalysisQueryForm form)
        {
            return RenderData(repository.SearchKeywords(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ITrendAnalysis>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Link([FromQuery] AnalysisQueryForm form)
        {
            return RenderData(repository.Host(form));
        }
    }
}
