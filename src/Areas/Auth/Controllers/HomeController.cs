using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetDream.Areas.Auth.Repositories;
using NetDream.Base.Helpers;

namespace NetDream.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class HomeController : Controller
    {
        private UserRepository _userRepository;

        public HomeController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login()
        {
            return Json(JsonResponse.Success());
        }
    }
}