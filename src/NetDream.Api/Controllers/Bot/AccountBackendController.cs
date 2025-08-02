using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.Bot
{
    [Route("open/bot/admin/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class AccountBackendController() : JsonController
    {

    }
}
