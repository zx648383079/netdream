using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Counter.Models;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Modules.Counter.Repositories;

namespace NetDream.Api.Controllers.Counter
{
    [Route("open/counter/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class StatisticsController(StatisticsRepository repository) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(SubtotalResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(string type = "today")
        {
            return Render(repository.Subtotal(type));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompareResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Trend(string type = "today", int compare = 0)
        {
            return Render(repository.TrendAnalysis(type, compare));
        }
    }
}
