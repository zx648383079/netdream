using Duende.IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDream.Api.Base.Http;
using NetDream.Api.Models;
using NetDream.Modules.Auth.Forms;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Shared.Interfaces.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetDream.Api.Controllers.Auth
{
    [ApiController]
    [Route("open/[controller]")]
    public class AuthController(IOptions<JwtSettings> _jwtSettingsAccessor, AuthRepository repository) : JsonController
    {
        //获取JwtSettings对象信息
        private readonly JwtSettings _jwtSettings = _jwtSettingsAccessor.Value;

        public IActionResult Login([FromBody] SignInForm form)
        {
            var res = repository.Login(form.GetContext());
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message, res.FailureReason);
            }
            var token = Token(res.Result);
            if (res.Result is IUserToken u)
            {
                u.Token = token;
            }
            return Render(res.Result);
        }

        [Authorize]
        [Route("get_user_info")]
        [HttpPost]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult GetUserInfo()
        {
            //获取当前请求用户的信息，包含 token信息
            var user = HttpContext.User;
            // user.Identity.Name;
            return Ok(user.Identity);
        }



        /// <summary>
        /// 获取 token
        /// </summary>
        /// <param name="user"></param>
        [NonAction]
        private string Token(IUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var authTime = DateTime.Now;//授权时间
            var expiresAt = authTime.AddDays(30);//过期时间
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new(JwtClaimTypes.Audience,_jwtSettings.Audience),
                    new(JwtClaimTypes.Issuer,_jwtSettings.Issuer),
                    new(JwtClaimTypes.Name, user.Id.ToString()),
                ]),
                Expires = expiresAt,
                //对称秘钥SymmetricSecurityKey
                //签名证书(秘钥，加密算法)SecurityAlgorithms
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
