using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.TradeTracker.Forms;
using NetDream.Modules.TradeTracker.Models;
using NetDream.Modules.TradeTracker.Repositories;

namespace NetDream.Api.Controllers.TradeTracker
{
    [Route("open/tracker/[controller]")]
    [ApiController]
    public class ProductController(ProductRepository repository) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ProductListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] ProductQueryForm form)
        {
            return RenderPage(repository.GetProductList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProductModel), 200)]
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
        [ProducesResponseType(typeof(DataResponse<TradeListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Price(int id)
        {
            return RenderData(repository.GetPrice(id));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<TradeListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Chart(int id, int channel, int type = 0,
                                string startAt = "", string endAt = "")
        {
            return RenderData(repository.GetPriceList(id, channel, type, startAt, endAt));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<string>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Suggest(string keywords)
        {
            return RenderData(repository.Suggest(keywords));
        }
    }
}
