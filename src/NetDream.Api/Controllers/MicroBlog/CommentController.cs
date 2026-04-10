using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Comment.Forms;
using NetDream.Modules.MicroBlog.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using CommentForm = NetDream.Modules.MicroBlog.Forms.CommentForm;

namespace NetDream.Api.Controllers.MicroBlog
{
    [Route("open/micro/[controller]")]
    [ApiController]
    public class CommentController(
        MicroRepository micro,
        ICommentRepository repository, IClientContext client) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ICommentItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] CommentQueryForm form)
        {
            return RenderPage(repository.Search(ModuleTargetType.MicroBlog, form.TargetId, form));
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] CommentForm form)
        {
            var res = micro.CommentSave(form);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpDelete]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            var res = repository.Remove(client.UserId, ModuleTargetType.Document, id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(AgreeResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Agree(int id)
        {
            var res = repository.Toggle(client.UserId, ModuleTargetType.Article, id, true);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }
        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(AgreeResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Disagree(int id)
        {
            var res = repository.Toggle(client.UserId, ModuleTargetType.Article, id, false);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }
    }
}
