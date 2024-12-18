using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Shared.Http;
using NetDream.Modules.OpenPlatform.Http;
using NPoco;

namespace NetDream.Web.Base.Http
{
    public abstract class JsonController : ControllerBase
    {

        public IJsonResponse JsonResponse
        {
            get {
                return HttpContext.Features.Get<IJsonResponse>() ?? new PlatformResponse();
            }
        }

        public IActionResult Render(object data) => Ok(JsonResponse.Render(data));

        public IActionResult RenderData<T>(T data) => Ok(JsonResponse.RenderData(data));

        public IActionResult RenderData<T>(T data, string message) => Ok(JsonResponse.RenderData(data, message));

        public IActionResult RenderPage<T>(Page<T> page) => Ok(JsonResponse.RenderPage(page));

        public IActionResult RenderFailure(string message, int code) => StatusCode(code, JsonResponse.RenderFailure(message, code));

        public IActionResult RenderFailure(string message) => StatusCode(404, JsonResponse.RenderFailure(message));
    }
}
