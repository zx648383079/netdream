using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.TradeTracker.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.TradeTracker
{
    [Route("open/tracker/[controller]")]
    [ApiController]
    public class ProductController(ProductRepository repository) : JsonController
    {
        public IActionResult Index([FromQuery] QueryForm form, int category = 0, int project = 0)
        {
            return RenderPage(repository.GetProductList(form, category, project));
        }
        [Route("[action]")]
        public IActionResult Detail(int id)
        {
            var res = repository.Get(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }
        [Route("[action]")]
        public IActionResult Price(int id)
        {
            return RenderData(repository.GetPrice(id));
        }
        [Route("[action]")]
        public IActionResult Chart(int id, int channel, int type = 0,
                                string startAt = "", string endAt = "")
        {
            return RenderData(repository.GetPriceList(id, channel, type, startAt, endAt));
        }
        [Route("[action]")]
        public IActionResult Suggest(string keywords)
        {
            return RenderData(repository.Suggest(keywords));
        }
    }
}
