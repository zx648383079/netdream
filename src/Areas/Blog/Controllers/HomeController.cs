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

        public IActionResult Index(int page = 1)
        {
            ViewData["categories"] = _repository.Categories();
            ViewData["items"] = _repository.GetPage(page);
            ViewData["newItems"] = _repository.GetNewBlogs(4);
            return View();
        }
    }
}