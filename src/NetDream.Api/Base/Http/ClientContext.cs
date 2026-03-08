using Duende.IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDream.Api.Models;
using NetDream.Modules.OpenPlatform.Http;
using NetDream.Shared.Helpers;
using NetDream.Shared.Http;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Api.Base.Http
{
    public class ClientContext(
        IHttpContextAccessor contextAccessor,
        IOptions<JwtSettings> jwtSettingsAccessor,
        IUserRepository userStore) : IClientContext
    {
        private readonly HttpContext? _context = contextAccessor.HttpContext;
        private IUserProfile? _currentUser;
        public string Ip {
            get {
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

        public string UserAgent => _context?.Request.Headers.UserAgent.ToString() ?? string.Empty;

        public string Host => _context?.Request.Host.Host ?? string.Empty;

        public int PlatformId {
            get {
                var res = _context?.Features.Get<IJsonResponse>();
                if (res is PlatformResponse o)
                {
                    return o.Platform?.Id ?? 0;
                }
                return 0;
            }
        }

        public int UserId {
            get {
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

        public Task LoginAsync(IUserProfile user)
        {
            _currentUser = user;
            //获取JwtSettings对象信息
            var jwtSettings = jwtSettingsAccessor.Value;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
            var authTime = DateTime.Now;//授权时间
            var expiresAt = authTime.AddDays(30);//过期时间
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new(JwtClaimTypes.Audience,jwtSettings.Audience),
                    new(JwtClaimTypes.Issuer,jwtSettings.Issuer),
                    new(JwtClaimTypes.Name, user.Id.ToString()),
                ]),
                Expires = expiresAt,
                //对称秘钥SymmetricSecurityKey
                //签名证书(秘钥，加密算法)SecurityAlgorithms
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            if (user is IUserToken u)
            {
                u.Token = tokenHandler.WriteToken(token);
            }
            return Task.CompletedTask;
        }
        public async Task<string> LogoutAsync()
        {
            _currentUser = null;
            var token = _context?.Request.Headers["Authorization"].ToString();
            await _context?.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
            return string.IsNullOrEmpty(token) 
                || !token.StartsWith(JwtBearerDefaults.AuthenticationScheme)
                ? string.Empty : token[JwtBearerDefaults.AuthenticationScheme.Length..].Trim();
        }
    }
}
