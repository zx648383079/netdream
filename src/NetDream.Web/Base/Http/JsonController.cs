using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetDream.Shared.Http;
using NetDream.Web.Base.Middleware;
using NPoco;
using System.Collections.Generic;
using System.Linq;

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
        [NonAction]
        public JsonResult Render(object data) => Json(JsonResponse.Render(data));
        [NonAction]
        public JsonResult RenderData<T>(T data) => Json(JsonResponse.RenderData(data));
        [NonAction]
        public JsonResult RenderData<T>(T data, string message) => Json(JsonResponse.RenderData(data, message));
        [NonAction]
        public JsonResult RenderPage<T>(Page<T> page) => Json(JsonResponse.RenderPage(page));
        [NonAction]
        public JsonResult RenderFailure(IEnumerable<KeyValuePair<string, ModelStateEntry?>> message)
        {
            var data = new Dictionary<string, IEnumerable<string>>();
            foreach (var item in message)
            {
                if (item.Value is null || item.Value.Errors.Count == 0)
                {
                    continue;
                }
                data.Add(item.Key, item.Value.Errors.Select(i => i.ErrorMessage));
            }
            return Json(JsonResponse.RenderFailure(data));
        }
        [NonAction]
        public JsonResult RenderFailure(string message, int code) => Json(JsonResponse.RenderFailure(message, code));
        [NonAction]
        public JsonResult RenderFailure(string message) => Json(JsonResponse.RenderFailure(message));
    }
}
