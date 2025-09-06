using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Counter.Entities;
using NetDream.Modules.Counter.Forms;
using NetDream.Modules.Counter.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.Counter
{
    [Route("open/counter/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class TrendController(AnalysisRepository repository) : JsonController
    {

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<LogEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Log([FromQuery] LogQueryForm form)
        {
            return RenderPage(repository.LogList(form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<int>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Log([FromBody] LogImportForm data, IFormFile file)
        {
            var res = repository.LogImport(data, new FormUploadFile(file));
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(AnalysisFlagEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult AnalysisMask([FromBody] AnalysisMarkForm data)
        {
            var res = repository.Mark(data);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }
    }
}
