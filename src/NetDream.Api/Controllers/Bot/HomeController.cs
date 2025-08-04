using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Bot.Repositories;

namespace NetDream.Api.Controllers.Bot
{
    [Route("open/bot")]
    [ApiController]
    public class HomeController(EmulateRepository repository) : JsonController
    {

    }
}
