﻿using Microsoft.AspNetCore.Mvc;
using NetDream.Areas.Gzo.Repositories;
using NetDream.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.Gzo.Controllers
{
    [Area("Gzo")]
    public class SqlController : Controller
    {
        private GzoRepository _repository;
        public SqlController(GzoRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return Json(JsonResponse.Success());
        }

        public IActionResult Table()
        {
            var data = _repository.AllTableNames();
            return Json(JsonResponse.Success(data));
        }
    }
}
