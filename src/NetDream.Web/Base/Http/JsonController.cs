using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Core.Http;
using NetDream.Web.Base.Middleware;
using NPoco;

namespace NetDream.Web.Base.Http
{
    public abstract class JsonController : Controller
    {

        public IJsonResponse JsonResponse
        {
            get
            {
                return HttpContext.Features.Get<IJsonResponse>() ?? new JsonResponse();
            }
        }

        public JsonResult Render(object data) => Json(JsonResponse.Render(data));

        public JsonResult RenderData<T>(T data) => Json(JsonResponse.RenderData(data));

        public JsonResult RenderData<T>(T data, string message) => Json(JsonResponse.RenderData(data, message));

        public JsonResult RenderPage<T>(Page<T> page) => Json(JsonResponse.RenderPage(page));

        public JsonResult RenderFailure(string message, int code) => Json(JsonResponse.RenderFailure(message, code));

        public JsonResult RenderFailure(string message) => Json(JsonResponse.RenderFailure(message));
    }
}
