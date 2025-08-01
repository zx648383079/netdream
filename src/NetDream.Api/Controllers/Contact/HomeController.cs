using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Career.Models;
using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Forms;
using NetDream.Modules.Contact.Repositories;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.Contact
{
    [Route("open/contact")]
    [ApiController]
    public class HomeController(ContactRepository repository) : JsonController
    {

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(FeedbackEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Feedback([FromBody] FeedbackForm form)
        {
            var res = repository.SaveFeedback(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(SubscribeEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Subscribe([FromBody] SubscribeForm form)
        {
            var res = repository.SaveSubscribe(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Unsubscribe(string email)
        {
            var res = repository.Unsubscribe(email);
            if (res.Succeeded)
            {
                return RenderData(true);
            }
            return RenderFailure(res.Message);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProfileFormatted), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Developer()
        {
            return Render(new ProfileFormatted());
        }
    }
}
