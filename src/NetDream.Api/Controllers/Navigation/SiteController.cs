using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Navigation.Entities;
using NetDream.Modules.Navigation.Forms;
using NetDream.Modules.Navigation.Repositories;
using NetDream.Modules.OpenPlatform.Models;

namespace NetDream.Api.Controllers.Navigation
{
    [Route("open/navigation/[controller]")]
    [ApiController]
    public class SiteController(SiteRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<SiteEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] SiteQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<SiteEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Recommend(int category = 0)
        {
            return RenderData(repository.Recommend(category));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(SiteEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.Get(id);
            if (!res.Successed)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(SiteEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Category()
        {
            return RenderData(repository.Categories());
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(SiteEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult CategoryRecommend(int category)
        {
            return RenderData(repository.RecommendGroup(category));
        }
    }
}
