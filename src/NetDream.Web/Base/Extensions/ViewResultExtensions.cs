﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace NetDream.Web.Base.Extensions
{
    public static class ViewResultExtensions
    {
        public static string ToHtml(this ViewResult result, HttpContext httpContext)
        {
            var feature = httpContext.Features.Get<IRoutingFeature>();
            var routeData = feature.RouteData;
            var viewName = result.ViewName ?? routeData.Values["action"] as string;
            var actionContext = new ActionContext(httpContext, routeData, new ControllerActionDescriptor());
            var options = httpContext.RequestServices.GetService<IOptions<MvcViewOptions>>();
            var htmlHelperOptions = options.Value.HtmlHelperOptions;
            var viewEngineResult = result.ViewEngine?.FindView(actionContext, viewName, true) ?? options.Value.ViewEngines.Select(x => x.FindView(actionContext, viewName, true)).FirstOrDefault(x => x != null);
            var view = viewEngineResult.View;
            var builder = new StringBuilder();

            using (var output = new StringWriter(builder))
            {
                var viewContext = new ViewContext(actionContext, view, result.ViewData, result.TempData, output, htmlHelperOptions);

                view
                    .RenderAsync(viewContext)
                    .GetAwaiter()
                    .GetResult();
            }

            return builder.ToString();
        }
    }
}
