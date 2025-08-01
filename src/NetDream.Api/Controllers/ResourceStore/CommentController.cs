using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.ResourceStore.Models;
using NetDream.Modules.ResourceStore.Repositories;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Forms;
using NetDream.Shared.Providers.Models;

namespace NetDream.Api.Controllers.ResourceStore
{
    [Route("open/res/[controller]")]
    [ApiController]
    public class CommentController(ResourceRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<CommentListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] CommentQueryForm form)
        {
            return RenderPage(repository.Comment().Search(form));
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(PageResponse<CommentListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] CommentForm form)
        {
            var res = repository.Comment().Insert(form);
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
            var res = repository.Comment().RemoveBySelf(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(PageResponse<AgreeResult>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Agree(int id)
        {
            var res = repository.Comment().Agree(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }
        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(PageResponse<AgreeResult>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Disagree(int id)
        {
            var res = repository.Comment().Agree(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ScoreListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Score([FromQuery] ScoreQueryForm form)
        {
            return RenderPage(repository.Score().Search(form));
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ScoreSubtotal), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult ScoreCount(int id)
        {
            return Render(repository.Score().Count(id));
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
