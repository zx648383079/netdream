using Microsoft.AspNetCore.Http;
using NetDream.Areas.Open.Http;
using NetDream.Areas.Open.Repositories;
using NetDream.Base.Http;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetDream.Base.Middlewares
{
    public class ResponseMiddleware
    {
        public const string RESPONSE_KEY = "json";

        private readonly RequestDelegate _next;

        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context, OpenRepository repository)
        {
            if (context.Request.Path.Value.StartsWith("/open/"))
            {
                var res = new PlatformResponse();
                if (!context.Request.Query.ContainsKey("appid"))
                {
                    
                    return JsonAsync(context, res.RenderFailure("APP ID error"), 404);
                }
                var appid = context.Request.Query["appid"];
                if (string.IsNullOrWhiteSpace(appid))
                {
                    return JsonAsync(context, res.RenderFailure("APP ID error"), 404);
                }
                var model = repository.GetByAppId(appid);
                if (model == null)
                {
                    return JsonAsync(context, res.RenderFailure("APP ID error"), 404);
                }
                if (!model.VerifyRule(context.Request.Path.Value))
                {
                    return JsonAsync(context, res.RenderFailure("The URL you requested was disabled"), 404);
                }
                if (!model.VerifyRest(context))
                {
                    return JsonAsync(context, res.RenderFailure("sign or encrypt error"), 404);
                }
                res.Platform = model;
                context.Items.Add(RESPONSE_KEY, res);
            } else
            {
                context.Items.Add(RESPONSE_KEY, new JsonResponse());
            }
            return _next.Invoke(context);
        }

        public Task JsonAsync(HttpContext context, object data, int code)
        {
            context.Response.StatusCode = code;
            return JsonAsync(context, data);
        }

        public Task JsonAsync(HttpContext context, object data)
        {
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonConvert.SerializeObject(data), Encoding.UTF8);
        }
    }
}
