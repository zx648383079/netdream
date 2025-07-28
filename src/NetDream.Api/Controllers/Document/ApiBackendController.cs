using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Document.Entities;
using NetDream.Modules.Document.Forms;
using NetDream.Modules.Document.Models;
using NetDream.Modules.Document.Repositories;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.Document
{
    [Route("open/doc/admin/api")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class ApiBackendController(ApiRepository repository, MockRepository mockStore) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(ApiModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.MangerGet(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<object>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Mock(int id)
        {
            var res = mockStore.GetMockData(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<RequestResult>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult DebugResult([FromBody] RequestForm form)
        {
            var res = mockStore.Request(form);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(res.Result);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<FieldEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Parse(string content, int kind = ApiRepository.KIND_REQUEST)
        {
            var res = mockStore.ParseContent(content, kind);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(res.Result);
        }
    }
}
