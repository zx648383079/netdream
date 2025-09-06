using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Counter.Forms;
using NetDream.Modules.Counter.Models;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.Counter
{
    [Route("open/counter/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class CustomController : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<ITrendAnalysis>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] AnalysisQueryForm form)
        {
            // TODO
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ITrendAnalysis>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult PageClick([FromQuery] AnalysisQueryForm form)
        {
            // TODO
            return RenderData(true);
        }
    }
}
