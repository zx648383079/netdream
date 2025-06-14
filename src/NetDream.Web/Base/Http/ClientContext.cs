using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using NetDream.Shared.Helpers;
using NetDream.Shared.Http;
using NetDream.Shared.Interfaces;
using NetDream.Modules.OpenPlatform.Http;
using NetDream.Shared.Interfaces.Entities;
using System.Diagnostics.CodeAnalysis;

namespace NetDream.Web.Base.Http
{
    public class ClientContext(
        IHttpContextAccessor contextAccessor,
        IUserRepository userStore
    ) : IClientContext
    {
        private readonly HttpContext? _context = contextAccessor.HttpContext;
        private IUserProfile? _currentUser;

        public string Ip 
        { 
            get 
            {
                if (_context is null)
                {
                    return string.Empty;
                }
                if (_context.Request.Headers.TryGetValue("X-Real-IP", out var value))
                {
                    return value.ToString() ?? string.Empty;
                }
                else if (_context.Request.Headers.TryGetValue("X-Forwarded-For", out var value1))
                {
                    return value1.ToString() ?? string.Empty;
                }
                else
                {
                    return _context.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;
                }
            } 
        }

        public string Language => _context?.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name ?? string.Empty;

        public string UserAgent => _context?.Request.Headers.UserAgent.ToString() ??string.Empty;

        public int PlatformId 
        {
            get {
                var res = _context?.Features.Get<IJsonResponse>();
                if (res is PlatformResponse o)
                {
                    return o.Platform?.Id ?? 0;
                }
                return 0;
            }
        }

        public int UserId 
        { 
            get 
            {
                var val = _context?.User.Identity?.Name;
                if (!string.IsNullOrWhiteSpace(val) && int.TryParse(val, out var userId))
                {
                    return userId;
                }
                return 0;
            } 
        }

        public string ClientName { get; private set; } = "web";

        public int Now { get; private set; } = TimeHelper.TimestampNow();

        public bool TryGetUser([NotNullWhen(true)] out IUserProfile? user)
        {
            var userId = UserId;
            if (userId <= 0)
            {
                user = null;
                return false;
            }
            if (_currentUser?.Id == userId)
            {
                user = _currentUser;
                return true;
            }
            user = _currentUser = userStore.GetProfile(userId);
            return user is not null;
        }
    }
}
