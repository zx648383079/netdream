using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Shared.Http;
using NetDream.Modules.OpenPlatform.Http;
using NetDream.Shared.Interfaces;

namespace NetDream.Api.Base.Http
{
    public abstract class JsonController : ControllerBase
    {
        public IJsonResponse JsonResponse
        {
            get {
                return HttpContext.Features.Get<IJsonResponse>() ?? new PlatformResponse();
            }
        }
        [NonAction]
        public IActionResult Render(object data) => Ok(JsonResponse.Render(data));

        [NonAction]
        public IActionResult RenderData<T>(T data) => Ok(JsonResponse.RenderData(data));
        [NonAction]
        public IActionResult RenderData<T>(T data, string message) => Ok(JsonResponse.RenderData(data, message));
        [NonAction]
        public IActionResult RenderPage<T>(IPage<T> page) => Ok(JsonResponse.RenderPage(page));
        [NonAction]
        public IActionResult RenderFailure(string message, int code) => StatusCode(code is > 300 and < 500 ? code : 404, JsonResponse.RenderFailure(message, code));
        [NonAction]
        public IActionResult RenderFailure(string message) => StatusCode(404, JsonResponse.RenderFailure(message));
        [NonAction]
        public IActionResult RenderFailure(IOperationResult message) => RenderFailure(message.Message, message.FailureReason);
    }
}
