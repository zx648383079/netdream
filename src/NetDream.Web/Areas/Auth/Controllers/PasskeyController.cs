﻿using Microsoft.AspNetCore.Mvc;

namespace NetDream.Web.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class PasskeyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
