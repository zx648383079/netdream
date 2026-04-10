using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.Wallet.Forms;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Modules.Wallet.Models;
using NetDream.Modules.Wallet.Repositories;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/admin/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class AccountBackendController(WalletRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<AccountLogListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] LogQueryForm form)
        {
            return RenderPage(repository.LogList(form));
        }
    }
}
