using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Comment.Forms;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.ResourceStore.Models;
using NetDream.Modules.ResourceStore.Repositories;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.ResourceStore
{
    [Route("open/res/[controller]")]
    [ApiController]
    public class CommentController(ResourceRepository repository, 
        ICommentRepository comment, IClientContext client) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ICommentItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] CommentQueryForm form)
        {
            return RenderPage(comment.Search(ModuleTargetType.ResourceStore, form.TargetId, form));
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] CommentForm form)
        {
            var res = comment.Create(client.UserId, ModuleTargetType.ResourceStore, form.TargetId, form);
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
            var res = comment.Remove(client.UserId, ModuleTargetType.ResourceStore, id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(AgreeResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Agree(int id)
        {
            var res = comment.Toggle(client.UserId, ModuleTargetType.ResourceStore, id, true);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }
        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(AgreeResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Disagree(int id)
        {
            var res = comment.Toggle(client.UserId, ModuleTargetType.ResourceStore, id, false);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }



        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IScoreSubtotal), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult ScoreCount(int id)
        {
            return Render(comment.Score(ModuleTargetType.ResourceStore, id));
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(PageResponse<ScoreModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Grade(int id, byte score)
        {
            var res = repository.GradeScore(id, score);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }
    }
}
