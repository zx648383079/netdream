using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NetDream.Modules.Auth.Forms;
using NetDream.Modules.Auth.Repositories;
using NetDream.Web.Base.Http;

namespace NetDream.Web.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class HomeController(AuthRepository repository, 
        IStringLocalizer<HomeController> localizer) : JsonController
    {
        public IActionResult Index()
        {
            ViewData["isCaptcha"] = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromForm] EmailSignInForm form, string redirect_uri = "/")
        {
            var res = await repository.LoginAsync(form);
            if (!res.Succeeded)
            {
                return Json(JsonResponse.RenderFailure(res.Message));
            }
            if (string.IsNullOrWhiteSpace(redirect_uri))
            {
                redirect_uri = "/";
            }
            return Json(JsonResponse.RenderData(new { 
                url = redirect_uri,
            }, localizer["登录成功！"]));
        }

        // [Authorize]
        // [Authorize(Roles = "Administrator")]
        public async Task<JsonResult> Logout()
        {
            await repository.LogoutAsync();
            return Json(JsonResponse.RenderData(true, "退出成功"));
        }
    }
}