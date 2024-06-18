﻿using Microsoft.AspNetCore.Http;
using NetDream.Core.Helpers;
using NetDream.Core.Interfaces;

namespace NetDream.Web.Base.Http
{
    public class ClientEnvironment : IClientEnvironment
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ClientEnvironment(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            Now = TimeHelper.TimestampNow();
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

        public int PlatformId => 0;
        public int UserId { get; private set; }

        public string ClientName { get; private set; } = "web";

        public int Now { get; private set; }
    }
}