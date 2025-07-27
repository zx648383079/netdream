using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Finance.Models;
using NetDream.Modules.Finance.Repositories;
using NetDream.Modules.OpenPlatform.Models;

namespace NetDream.Api.Controllers.Finance
{
    [Route("open/finance/[controller]")]
    [Authorize()]
    [ApiController]
    public class IncomeController(StatisticsRepository repository) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IncomeStatisticsResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(string month = "")
        {
            return Render(repository.GetIncome(month));
        }
    }
}
