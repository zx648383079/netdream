using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Finance.Forms;
using NetDream.Modules.Finance.Models;
using NetDream.Modules.Finance.Repositories;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.Finance
{
    [Route("open/finance/[controller]")]
    [Authorize()]
    [ApiController]
    public class StatisticsController(StatisticsRepository repository) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(StatisticsResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] StatisticsForm form)
        {
            return Render(repository.Subtotal(form));
        }
    }
}
