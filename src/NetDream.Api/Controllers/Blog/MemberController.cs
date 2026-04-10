using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Article.Entities;
using NetDream.Modules.Article.Forms;
using NetDream.Modules.Article.Models;
using NetDream.Modules.Article.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Interfaces;

namespace NetDream.Api.Controllers.Blog
{
    [Route("open/blog")]
    [ApiController]
    public class MemberController(
        PublishRepository repository,
        IClientContext client,
        IStorageRepository fileStore) : JsonController
    {
        [Route("publish/page")]
        [Authorize]
        public IActionResult Index([FromQuery] BlogQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("publish/[action]")]
        [ProducesResponseType(typeof(PageResponse<ArticleModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id, string language = "")
        {
            var res = repository.Get(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }


        [HttpPost]
        [Route("publish")]
        [ProducesResponseType(typeof(ArticleEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] ArticleForm form)
        {
            var res = repository.Save(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("publish/[action]")]
        [ProducesResponseType(typeof(ArticleEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SaveDraft([FromBody] ArticleForm form)
        {
            var res = repository.SaveDraft(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("publish/[action]")]
        [ProducesResponseType(typeof(IFileListItem), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Upload(IFormFile file)
        {
            var res = fileStore.UploadImage(client.UserId, new FormUploadFile(file));
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpDelete]
        [Route("publish/[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            var res = repository.Remove(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [Route("home/edit_option")]
        [Authorize]
        public IActionResult Option()
        {
            return Render(null);
        }
    }
}
