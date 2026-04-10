using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Article.Forms;
using NetDream.Modules.Article.Models;
using NetDream.Modules.Article.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Blog
{
    [Route("open/blog")]
    [ApiController]
    public class HomeController(BlogRepository repository) : JsonController
    {
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ArticleListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] BlogQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ArticleOpenModel>), 200)]
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
        [ProducesResponseType(typeof(PageResponse<ArticleOpenModel>), 200)]
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
        [ProducesResponseType(typeof(PageResponse<ArticleOpenModel>), 200)]
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
        [ProducesResponseType(typeof(PageResponse<ArticleOpenModel>), 200)]
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
        [ProducesResponseType(typeof(DataResponse<ArchiveListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Archives()
        {
            return RenderData(repository.GetArchives());
        }

        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<StatisticsItem>), 200)]
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
