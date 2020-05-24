﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetDream.Areas.Gzo.Repositories;
using NetDream.Base.Helpers;

namespace NetDream.Areas.Gzo.Controllers
{
    [Area("Gzo")]
    public class HomeController : Controller
    {
        private GzoRepository _repository;
        public HomeController(GzoRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Model()
        {
            return View();
        }
    }
}