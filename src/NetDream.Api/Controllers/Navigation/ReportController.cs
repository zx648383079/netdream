using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Contact.Forms;
using NetDream.Modules.Contact.Repositories;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.Navigation
{
    [Route("open/navigation/[controller]")]
    [ApiController]
    public class ReportController(ReportRepository repository) : JsonController
    {
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromBody] ReportForm form)
        {
            var res = repository.Create(form);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }
    }
}
