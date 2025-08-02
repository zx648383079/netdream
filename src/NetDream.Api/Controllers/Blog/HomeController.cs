using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Blog.Forms;
using NetDream.Modules.Blog.Models;
using NetDream.Modules.Blog.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Models;

namespace NetDream.Api.Controllers.Blog
{
    [Route("open/blog")]
    [ApiController]
    public class HomeController(BlogRepository repository) : JsonController
    {
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<BlogListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] BlogQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<BlogModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id, string open_key = "")
        {
            var res = repository.Get(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<BlogModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Content(int id, string open_key = "")
        {
            var res = repository.GetBody(id, open_key);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<BlogModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Open(int id, string open_key = "")
        {
            var res = repository.OpenBody(id, open_key);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<BlogModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Recommend(int id)
        {
            var res = repository.Recommend(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<BlogArchiveItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Archives()
        {
            return RenderData(repository.GetArchives());
        }

        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<TagListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Tag()
        {
            return RenderData(repository.GetTags());
        }


        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<ListArticleItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Suggest(string keywords = "")
        {
            return RenderData(repository.Suggest(keywords));
        }
    }
}
