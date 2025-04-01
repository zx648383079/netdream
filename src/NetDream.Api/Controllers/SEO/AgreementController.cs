using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.SEO.Repositories;

namespace NetDream.Api.Controllers.SEO
{
    [Route("open/seo")]
    [ApiController]
    public class AgreementController(AgreementRepository repository) : JsonController
    {

        [Route("[controller]")]
        [HttpGet]
        public IActionResult Index(string name)
        {
            var res = repository.GetByName(name);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }
    }
}
