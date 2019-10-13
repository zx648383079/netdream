using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetDream.Areas.Blog.Repositories;

namespace NetDream.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class HomeController : Controller
    {
        private BlogRepository _repository;
        public HomeController(BlogRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {

            return View();
        }
    }
}