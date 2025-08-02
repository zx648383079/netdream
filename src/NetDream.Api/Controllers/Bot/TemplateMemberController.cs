using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;

namespace NetDream.Api.Controllers.Bot
{
    [Route("open/bot/member/[controller]")]
    [Authorize]
    [ApiController]
    public class TemplateMemberController() : JsonController
    {

    }
}
