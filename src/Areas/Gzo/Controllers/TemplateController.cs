using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Areas.Gzo.Repositories;
using NetDream.Base.Helpers;
using NetDream.Base.Http;

namespace NetDream.Areas.Gzo.Controllers
{
    [Area("Gzo")]
    public class TemplateController : JsonController
    {
        private GzoRepository _repository;
        public TemplateController(GzoRepository repository, IHttpContextAccessor accessor) : base(accessor)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Model(string table, bool preview = false)
        {
            if (preview)
            {
                return Json(JsonResponse.RenderData(new
                {
                    code = _repository.Generate(table)
                }));
            }
            return Json(JsonResponse.RenderFailure("未实现"));
        }
    }
}