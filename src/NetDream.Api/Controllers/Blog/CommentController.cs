using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Comment.Forms;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Blog
{
    [Route("open/blog/[controller]")]
    [Authorize]
    [ApiController]
    public class CommentController(
        ICommentRepository repository,
        IClientContext client
        ) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ICommentItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] CommentQueryForm form)
        {
            return RenderPage(repository.Search(ModuleTargetType.Article, form.TargetId, form));
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] CommentForm form)
        {
            var res = repository.Create(client.UserId, ModuleTargetType.Article, form.TargetId, form);
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
            var res = repository.Remove(client.UserId, ModuleTargetType.Article, id);
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

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Report(int id)
        {
            var res = repository.Report(client.UserId, ModuleTargetType.Article, id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(IUser), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Commentator(string email)
        {
            var res = repository.LastCommentator(email);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }
    }
}
