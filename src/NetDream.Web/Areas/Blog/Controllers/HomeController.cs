using Microsoft.AspNetCore.Mvc;
using NetDream.Web.Areas.Blog.Repositories;

namespace NetDream.Web.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class HomeController : Controller
    {
        private readonly BlogRepository _repository;
        public HomeController(BlogRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index(int page = 1)
        {
            ViewData["categories"] = _repository.Categories();
            ViewData["items"] = _repository.GetPage(page);
            ViewData["newItems"] = _repository.GetNewBlogs(4);
            ViewData["fullUrl"] = $"{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            ViewData["pageIndex"] = page;
            return View();
        }

        public IActionResult Detail(int id)
        {
            ViewData["categories"] = _repository.Categories();
            ViewData["data"] = _repository.GetBlog(id);
            ViewData["fullUrl"] = $"{HttpContext.Request.PathBase}{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            return View();
        }
    }
}