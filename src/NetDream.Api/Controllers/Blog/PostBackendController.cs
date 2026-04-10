using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Article.Forms;
using NetDream.Modules.Article.Models;
using NetDream.Modules.Article.Repositories;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.Blog
{
    [Route("open/blog/admin/blog")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class PostBackendController(BlogRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ArticleListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] ArticleQueryForm form)
        {
            return RenderPage(repository. AdvancedList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ArticleModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.AdvancedGet(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }


        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository. AdvancedRemove(id);
            return RenderData(true);
        }
    }
}
