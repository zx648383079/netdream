using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetDream.Areas.Gzo.Repositories;
using NetDream.Base.Helpers;

namespace NetDream.Areas.Gzo.Controllers
{
    [Area("Gzo")]
    public class TemplateController : Controller
    {
        private GzoRepository _repository;
        public TemplateController(GzoRepository repository)
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
                return Json(JsonResponse.Success(new
                {
                    code = _repository.Generate(table)
                }));
            }
            return Json(JsonResponse.Failure("未实现"));
        }
    }
}