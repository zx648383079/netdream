using Microsoft.AspNetCore.Mvc;
using NetDream.Modules.Blog.Repositories;

namespace NetDream.Web.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class TagController : Controller
    {
        private readonly BlogRepository _repository;
        public TagController(BlogRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            ViewData["items"] = _repository.GetTags();
            return View();
        }
    }
}
