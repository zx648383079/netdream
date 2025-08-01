using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Modules.TradeTracker.Repositories;
using NetDream.Modules.TradeTracker.Models;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.TradeTracker
{
    [Route("open/tracker/admin/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class StatisticsController(StatisticsRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<StatisticsResult>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index()
        {
            return Render(repository.Subtotal());
        }
    }
}
