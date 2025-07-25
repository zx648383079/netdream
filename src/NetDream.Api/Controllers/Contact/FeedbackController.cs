using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Repositories;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Contact
{
    [Route("open/contact")]
    [ApiController]
    public class FeedbackController(FeedbackRepository repository) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<FeedbackEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }
    }
}
