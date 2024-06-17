using Microsoft.AspNetCore.Mvc;
using NetDream.Modules.Gzo.Repositories;
using NetDream.Web.Base.Http;

namespace NetDream.Web.Areas.Gzo.Controllers
{
    [Area("Gzo")]
    public class SqlController(GzoRepository repository) : JsonController
    {
        public IActionResult Index()
        {
            return RenderData<object>(null);
        }

        public IActionResult Table()
        {
            var data = repository.AllTableNames();
            return RenderData(data);
        }
    }
}
