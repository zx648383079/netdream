using Microsoft.AspNetCore.Mvc;
using NetDream.Web.Areas.Blog.Repositories;

namespace NetDream.Web.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class ArchivesController : Controller
    {
        private readonly BlogRepository _repository;
        public ArchivesController(BlogRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            ViewData["items"] = _repository.GetArchives();
            return View();
        }
    }
}
