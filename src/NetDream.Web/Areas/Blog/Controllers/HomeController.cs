using Microsoft.AspNetCore.Mvc;
using NetDream.Modules.Blog.Forms;
using NetDream.Modules.Blog.Repositories;

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

        public IActionResult Index([FromQuery] BlogQueryForm form)
        {
            ViewData["categories"] = _repository.Categories();
            ViewData["items"] = _repository.GetList(form);
            ViewData["newItems"] = _repository.GetNewBlogs(4);
            ViewData["fullUrl"] = $"{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            ViewData["pageIndex"] = form.Page;
            return View();
        }

        public IActionResult Detail(int id)
        {
            ViewData["categories"] = _repository.Categories();
            ViewData["data"] = _repository.Get(id).Result;
            ViewData["fullUrl"] = $"{HttpContext.Request.PathBase}{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            return View();
        }
    }
}