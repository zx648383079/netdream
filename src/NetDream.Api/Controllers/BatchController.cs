using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Web.Base.Http;

namespace NetDream.Api.Controllers
{
    [Route("open/[controller]")]
    [ApiController]
    public class BatchController : JsonController
    {
        [HttpPost]
        public IActionResult Index()
        {

        }
    }
}
