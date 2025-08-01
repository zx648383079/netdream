using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Contact.Models;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Modules.Contact.Repositories;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.Contact
{

    [Route("open/contact/admin/[controller]")]
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
