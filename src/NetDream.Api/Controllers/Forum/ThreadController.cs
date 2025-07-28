using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Forum.Entities;
using NetDream.Modules.Forum.Forms;
using NetDream.Modules.Forum.Models;
using NetDream.Modules.Forum.Repositories;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Shared.Models;
using NetDream.Shared.Notifications;

namespace NetDream.Api.Controllers.Forum
{

    [Route("open/forum")]
    [ApiController]
    public class ThreadController(ThreadRepository repository) : JsonController
    {
        [HttpGet]
        [Route("[controller]")]
        [ProducesResponseType(typeof(PageResponse<ThreadListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] ThreadQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [ProducesResponseType(typeof(PageResponse<ThreadListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Post([FromQuery] PostQueryForm form)
        {
            return RenderPage(repository.PostList(form));
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [ProducesResponseType(typeof(ThreadModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.GetFull(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }


        [HttpPost]
        [Route("[controller]/[action]")]
        [Authorize]
        [ProducesResponseType(typeof(ThreadPostEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Create([FromBody] ThreadPublishForm form)
        {
            var res = repository.Create(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        [Authorize]
        [ProducesResponseType(typeof(ThreadEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Update([FromBody] ThreadForm form)
        {
            var res = repository.Update(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [Authorize]
        [ProducesResponseType(typeof(ThreadModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Edit(int id)
        {
            var res = repository.GetSource(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpGet]
        [Route("[controller]/user")]
        [ProducesResponseType(typeof(UserProfileCard), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult GetUser(int id)
        {
            var res = repository.GetUser(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [ProducesResponseType(typeof(Page<ThreadLogModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Reward([FromQuery] RewardQueryForm form)
        {
            return RenderPage(repository.RewardList(form));
        }

        [HttpDelete]
        [Route("[controller]/[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            var res = repository.Remove(id);
            if (res.Succeeded)
            {
                return RenderData(true);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        [Authorize]
        [ProducesResponseType(typeof(ThreadEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Reply([FromBody] ThreadReplyForm form)
        {
            var res = repository.Reply(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        [Authorize]
        [ProducesResponseType(typeof(ThreadEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Digest(int id)
        {
            var res = repository.ThreadAction(id, ["is_digest"]);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        [Authorize]
        [ProducesResponseType(typeof(ThreadEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Highlight(int id)
        {
            var res = repository.ThreadAction(id, ["is_highlight"]);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        [Authorize]
        [ProducesResponseType(typeof(ThreadEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Close(int id)
        {
            var res = repository.ThreadAction(id, ["is_closed"]);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpDelete]
        [Route("[controller]/[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult DeletePost(int id)
        {
            var res = repository.RemovePost(id);
            if (res.Succeeded)
            {
                return RenderData(true);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        [Authorize]
        [ProducesResponseType(typeof(ThreadPostEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult ChangePost(int id, byte status = 0)
        {
            var res = repository.ChangePost(id, status);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Collect(int id)
        {
            var res = repository.ToggleCollect(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<AgreeResult>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Agree(int id)
        {
            var res = repository.AgreePost(id);
            if (res.Succeeded)
            {
                return RenderData(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<AgreeResult>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Disagree(int id)
        {
            var res = repository.AgreePost(id);
            if (res.Succeeded)
            {
                return RenderData(res.Result);
            }
            return RenderFailure(res.Message);
        }

      

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ListArticleItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Suggestion(string keywords)
        {
            return RenderData(repository.Suggestion(keywords));
        }
    }
}
