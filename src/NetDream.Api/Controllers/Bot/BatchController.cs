using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;

namespace NetDream.Api.Controllers.Bot
{
    [Route("open/bot/[controller]")]
    [Authorize]
    [ApiController]
    public class BatchController() : JsonController
    {

    }
}
