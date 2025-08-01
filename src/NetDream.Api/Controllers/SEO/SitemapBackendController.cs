using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Notifications;
using System.Threading.Tasks;

namespace NetDream.Api.Controllers.SEO
{
    [Route("open/seo/admin/sitemap")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class SitemapBackendController(IMediator mediator) : JsonController
    {
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<SitemapNode>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public async Task<IActionResult> Index()
        {
            var res = new SitemapRequest();
            await mediator.Publish(res);
            return RenderData(res.Items);
        }
    }
}
