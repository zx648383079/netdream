using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.MicroBlog.Forms;
using NetDream.Modules.MicroBlog.Models;
using NetDream.Modules.MicroBlog.Repositories;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.MicroBlog
{
    [Route("open/micro/[controller]")]
    [Authorize]
    [ApiController]
    public class PublishController(MicroRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<PostListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderPage(repository.GetSelfList(form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PostListItem), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Create([FromBody] PublishForm form)
        {
            if (!repository.CanPublish())
            {
                return RenderFailure("发送过于频繁！");
            }
            var res = repository.Create(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.RemoveSelf(id);
            return RenderData(true);
        }
    }
}
