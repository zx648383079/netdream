﻿using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDream.Api.Models;
using NetDream.Modules.Auth.Models;
using NetDream.Modules.OpenPlatform.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetDream.Api.Controllers.Auth
{
    [ApiController]
    [Route("open/[controller]")]
    public class AuthController : ControllerBase
    {
        //获取JwtSettings对象信息
        private readonly JwtSettings _jwtSettings;
        public AuthController(IOptions<JwtSettings> _jwtSettingsAccesser)
        {
            _jwtSettings = _jwtSettingsAccesser.Value;
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
        private object Token(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var authTime = DateTime.Now;//授权时间
            var expiresAt = authTime.AddDays(30);//过期时间
            var tokenDescripor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(JwtClaimTypes.Audience,_jwtSettings.Audience),
                    new Claim(JwtClaimTypes.Issuer,_jwtSettings.Issuer),
                    new Claim(JwtClaimTypes.Name, user.Id.ToString()),
                }),
                Expires = expiresAt,
                //对称秘钥SymmetricSecurityKey
                //签名证书(秘钥，加密算法)SecurityAlgorithms
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescripor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
