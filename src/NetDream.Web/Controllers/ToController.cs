using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetDream.Web.Base.Helpers;
using NetDream.Core.Helpers;
using NetDream.Web.Models;

namespace NetDream.Web.Controllers
{
    public class ToController : Controller
    {
        public IActionResult Index(string url = "")
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                url = StrHelper.Base64Decode(url + "=");
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
