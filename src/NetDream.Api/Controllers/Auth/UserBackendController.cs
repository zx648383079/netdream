using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/admin/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class UserBackendController : JsonController
    {
    }
}
