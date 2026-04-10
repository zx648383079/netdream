using Microsoft.AspNetCore.Mvc;
using NetDream.Modules.Article.Repositories;

namespace NetDream.Web.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class CategoryController : Controller
    {
        private readonly BlogRepository _repository;
        public CategoryController(BlogRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            ViewData["items"] = _repository.Categories();
            return View();
        }
    }
}
