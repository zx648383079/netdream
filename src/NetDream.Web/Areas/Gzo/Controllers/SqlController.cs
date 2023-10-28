using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Modules.Gzo.Repositories;
using NetDream.Web.Base.Helpers;
using NetDream.Web.Base.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Gzo.Controllers
{
    [Area("Gzo")]
    public class SqlController : JsonController
    {
        private readonly GzoRepository _repository;
        public SqlController(GzoRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return Json(JsonResponse.RenderData<object>(null));
        }

        public IActionResult Table()
        {
            var data = _repository.AllTableNames();
            return Json(JsonResponse.RenderData(data));
        }
    }
}
