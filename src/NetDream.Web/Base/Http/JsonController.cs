using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Web.Base.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Base.Http
{
    public abstract class JsonController : Controller
    {

        public IJsonResponse JsonResponse
        {
            get
            {
                return HttpContext.Items.ContainsKey(ResponseMiddleware.RESPONSE_KEY) ? new JsonResponse() : HttpContext.Items[ResponseMiddleware.RESPONSE_KEY] as IJsonResponse;
            }
        }
    }
}
