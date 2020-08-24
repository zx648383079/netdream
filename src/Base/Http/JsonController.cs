using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Base.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Base.Http
{
    public abstract class JsonController : Controller
    {
        protected IHttpContextAccessor contextAccessor;

        public JsonController(IHttpContextAccessor accessor)
        {
            contextAccessor = accessor;
        }

        public IJsonResponse JsonResponse
        {
            get
            {
                return contextAccessor == null || 
                    !contextAccessor.HttpContext.Items.ContainsKey(ResponseMiddleware.RESPONSE_KEY) ? new JsonResponse() : contextAccessor.HttpContext.Items[ResponseMiddleware.RESPONSE_KEY] as IJsonResponse;
            }
        }
    }
}
