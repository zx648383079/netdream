using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.MicroBlog.Forms;
using NetDream.Modules.MicroBlog.Repositories;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.OpenPlatform.Repositories;

namespace NetDream.Api.Controllers.MicroBlog
{
    [Route("open/micro/[controller]")]
    [Authorize]
    [ApiController]
    public class ShareController(OpenRepository open, MicroRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(string shareappid, string url = "")
        {
            var res = open.CheckUrl(shareappid, url);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] ShareForm form)
        {
            if (!repository.CanPublish())
            {
                return RenderFailure("发送过于频繁！");
            }
            var res = open.CheckUrl(form.Shareappid, form.Url);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            var next = repository.Share(form);
            if (!next.Succeeded)
            {
                return RenderFailure(next.Message);
            }
            return Render(next.Result);
        }
    }
}
