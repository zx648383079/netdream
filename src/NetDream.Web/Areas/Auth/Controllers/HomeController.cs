using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Modules.Auth.Repositories;
using NetDream.Web.Base.Http;

namespace NetDream.Web.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class HomeController : JsonController
    {
        private readonly UserRepository _userRepository;

        public HomeController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            Console.WriteLine();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(string email, string password, string redirect_uri = "/")
        {
            var user = _userRepository.Login(email, password);
            if (user == null)
            {
                return Json(JsonResponse.RenderFailure("邮箱或密码不正确"));
            }
            if (string.IsNullOrWhiteSpace(redirect_uri))
            {
                redirect_uri = "/";
            }
            var claims = new List<Claim>(){
                new Claim(ClaimTypes.Name, user.Id.ToString()),
            };

            //init the identity instances 
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Customer"));
            // sign in 
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                IsPersistent = false,
                AllowRefresh = false
            });
            return Json(JsonResponse.RenderData(new { 
                url = redirect_uri,
            }, "登录成功！"));
        }


        public async Task<JsonResult> Logout()
        {
            var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (auth.Succeeded)
            {
                var userId = auth.Principal.Identity.Name;
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Json(JsonResponse.RenderData(null, "退出成功"));
        }
    }
}