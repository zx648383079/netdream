using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Finance.Models;
using NetDream.Modules.Finance.Repositories;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.Finance
{
    [Route("open/finance/[controller]")]
    [Authorize()]
    [ApiController]
    public class MoneyController(StatisticsRepository repository) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(MoneyStatisticsResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index()
        {
            return Render(repository.GetMoney());
        }
    }
}
