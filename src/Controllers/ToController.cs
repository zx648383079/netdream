using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetDream.Base.Helpers;
using NetDream.Models;

namespace NetDream.Controllers
{
    public class ToController : Controller
    {
        public IActionResult Index(string url = "")
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                url = Str.Base64Decode(url + "=");
            }
            if (string.IsNullOrEmpty(url))
            {
                url = "/";
            }
            ViewData["url"] = url;
            return View();
        }
    }
}
