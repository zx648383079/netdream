using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.AdSense.Forms;
using NetDream.Modules.AdSense.Models;
using NetDream.Modules.AdSense.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.AdSense
{
    [Route("open/ad/admin/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class StatisticsController(StatisticsRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(StatisticsResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] AdQueryForm form)
        {
            return Render(repository.Subtotal());
        }
    }
}
