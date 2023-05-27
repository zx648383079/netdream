using Microsoft.AspNetCore.Http;
using NetDream.Api.Base.Middleware;
using NetDream.Core.Helpers;
using NetDream.Core.Interfaces;
using NetDream.Modules.OpenPlatform.Http;

namespace NetDream.Api.Base.Http
{
    public class ClientEnvironment : IClientEnvironment
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ClientEnvironment(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            Now = Time.TimestampNow();
            var val = _contextAccessor.HttpContext.User.Identity.Name;
            if (!string.IsNullOrWhiteSpace(val) && int.TryParse(val, out var userId))
            {
                UserId = userId;
            }
            if (_contextAccessor.HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
            {
                Ip = _contextAccessor.HttpContext.Request.Headers["X-Real-IP"];
            } else if (_contextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                Ip = _contextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
            } else
            {
                Ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }

        public string Ip { get; private set; }

        public string UserAgent => _contextAccessor.HttpContext.Request.Headers.UserAgent;

        public int PlatformId {
            get {
                if (_contextAccessor.HttpContext.Items.TryGetValue(ResponseMiddleware.RESPONSE_KEY, out var json))
                {
                    if (json is PlatformResponse o && o.Platform is not null)
                    {
                        return o.Platform.Id;
                    }
                }
                return 0;
            }
        }
        public int UserId { get; private set; }

        public string ClientName { get; private set; } = "web";

        public int Now { get; private set; }
    }
}
