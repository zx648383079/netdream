using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.SEO.Repositories;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Repositories.Models;

namespace NetDream.Api.Controllers.SEO
{
    [Route("open/seo/admin/sql")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class SqlBackendController(SEORepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<FileItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index()
        {
            return RenderData(repository.SqlFiles());
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult BackUp()
        {
            var res = repository.BackUpSql(true);
            if (res.Succeeded)
            {
                return RenderData(true);
            }
            return RenderFailure(res.Message);
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Clear()
        {
            repository.ClearSql();
            return RenderData(true);
        }
    }
}
