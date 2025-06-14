using Microsoft.AspNetCore.Mvc;
using NetDream.Modules.Gzo.Repositories;
using NetDream.Web.Base.Http;

namespace NetDream.Web.Areas.Gzo.Controllers
{
    [Area("Gzo")]
    public class TemplateController(GzoRepository repository, CodeRepository code) : JsonController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Model(string table, bool preview = false)
        {
            if (preview)
            {
                return RenderData(new
                {
                    code = repository.PreviewEntity(table)
                });
            }
            return RenderFailure("未实现");
        }

        public IActionResult Exchange(string content, string source = "", string target = "")
        {
            return RenderData(code.Exchange(content, source, target));
        }
    }
}