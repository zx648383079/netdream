using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetDream.Modules.Gzo.Repositories;
using NetDream.Web.Base.Helpers;

namespace NetDream.Web.Areas.Gzo.Controllers
{
    [Area("Gzo")]
    public class HomeController(GzoRepository repository) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Model()
        {
            return View();
        }

        public IActionResult Exchange()
        {
            return View();
        }
    }
}