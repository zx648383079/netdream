using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Models;
using NetDream.Modules.Blog.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Forms;
using CommentForm = NetDream.Modules.Blog.Forms.CommentForm;

namespace NetDream.Api.Controllers.Blog
{
    [Route("open/blog/[controller]")]
    [Authorize]
    [ApiController]
    public class CommentController(CommentRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<CommentListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] CommentQueryForm form, bool is_hot = false)
        {
            return RenderPage(repository.GetList(form, is_hot));
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(PageResponse<CommentEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] CommentForm form)
        {
            var res = repository.Create(form);
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
            var res = repository.RemoveSelf(id);
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
            var res = repository.Agree(id);
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
            var res = repository.Disagree(id);
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
            var res = repository.Report(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CommentEntity), 200)]
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
