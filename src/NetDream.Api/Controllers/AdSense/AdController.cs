using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.AdSense.Entities;
using NetDream.Modules.AdSense.Models;
using NetDream.Modules.AdSense.Repositories;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.AdSense
{
    [Route("open/[controller]")]
    [ApiController]
    public class AdController(AdRepository repository) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<AdEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(string position)
        {
            return RenderData(repository.GetList(string.Empty, position));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AdEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.Get(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<FormattedAdModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Banner()
        {
            return RenderData(repository.MobileBanners());
        }
    }
}
