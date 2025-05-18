using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.TradeTracker.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.TradeTracker
{
    [Route("open/tracker/[controller]")]
    [ApiController]
    public class LogController(TrackRepository repository) : JsonController
    {
        public IActionResult Index([FromQuery] QueryForm form, int channel = 0, int project = 0, int product = 0)
        {
            return RenderPage(repository.LatestList(form, channel, project, product));
        }

        [Route("[action]")]
        public IActionResult Batch(string[] product, string to, string channel = "")
        {
            return RenderData(repository.BatchLatestList(channel, product, to));
        }
    }
}
