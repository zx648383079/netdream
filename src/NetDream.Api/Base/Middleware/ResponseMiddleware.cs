﻿using Microsoft.AspNetCore.Http;
using NetDream.Shared.Http;
using NetDream.Modules.OpenPlatform.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.OpenPlatform.Repositories;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Api.Base.Middleware
{
    public class ResponseMiddleware(RequestDelegate next)
    {
        public Task InvokeAsync(HttpContext context, OpenRepository repository)
        {
            var res = new PlatformResponse();
            if (!context.Request.Query.ContainsKey("appid"))
            {
                return JsonAsync(context, res.RenderFailure("APP ID error"), 404);
            }
            var appId = context.Request.Query["appid"];
            if (string.IsNullOrWhiteSpace(appId))
            {
                return JsonAsync(context, res.RenderFailure("APP ID error"), 404);
            }
            var model = repository.GetByAppId(appId);
            if (model == null)
            {
                return JsonAsync(context, res.RenderFailure("APP ID error"), 404);
            }
            if (!model.VerifyRule(context.Request.Path.Value))
            {
                return JsonAsync(context, res.RenderFailure("The URL you requested was disabled"), 404);
            }
            if (!VerifyRest(model, context))
            {
                return JsonAsync(context, res.RenderFailure("sign or encrypt error"), 404);
            }
            res.Platform = model;
            context.Features.Set<IJsonResponse>(res);
            return next.Invoke(context);
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

        public bool VerifyRest(PlatformModel model, HttpContext context)
        {
            if (model.SignType < 1)
            {
                return true;
            }
            if (model.SignType == 1)
            {
                var data = new Dictionary<string, string>();
                foreach (var item in context.Request.Query)
                {
                    if (item.Key == "sign")
                    {
                        continue;
                    }
                    data.Add(item.Key, item.Value.ToString());
                }
                return model.Sign(data) == context.Request.Query["sign"].ToString();
            }
            return false;
        }
    }
}
