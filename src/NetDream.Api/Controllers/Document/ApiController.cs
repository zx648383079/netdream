using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Document.Models;
using NetDream.Modules.Document.Repositories;
using NetDream.Modules.OpenPlatform.Models;

namespace NetDream.Api.Controllers.Document
{
    [Route("open/doc/[controller]")]
    [ApiController]
    public class ApiController(ApiRepository repository, MockRepository mockStore) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IPageModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(int id)
        {
            var res = repository.GetRead(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<string>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Language()
        {
            return RenderData<string[]>([]);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<string>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Mock(int id)
        {
            if (!repository.CanOpen(id))
            {
                return RenderFailure("无权限浏览");
            }
            return RenderData<string[]>([]);
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<string>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Code(int id, string lang, int kind = ApiRepository.KIND_RESPONSE)
        {
            if (!repository.CanOpen(id))
            {
                return RenderFailure("无权限浏览");
            }
            return RenderData<string[]>([]);
        }
    }
}
