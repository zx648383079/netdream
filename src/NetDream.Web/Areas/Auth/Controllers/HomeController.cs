using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Modules.Auth.Forms;
using NetDream.Modules.Auth.Repositories;
using NetDream.Web.Base.Http;

namespace NetDream.Web.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class HomeController(AuthRepository repository) : JsonController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([Bind("email", "password")] EmailSignInForm form, string redirect_uri = "/")
        {
            var res = repository.Login(form);
            if (!res.IsSuccess)
            {
                return Json(JsonResponse.RenderFailure(res.Message));
            }
            var user = res.Result!;
            if (string.IsNullOrWhiteSpace(redirect_uri))
            {
                redirect_uri = "/";
            }
            var claims = new List<Claim>(){
                new(ClaimTypes.Name, user.Id.ToString()),
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
                var userId = auth.Principal.Identity?.Name;
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Json(JsonResponse.RenderData(true, "退出成功"));
        }
    }
}