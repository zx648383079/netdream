﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Models;
using NetDream.Web.Base.Http;

namespace NetDream.Api.Controllers
{
    [Route("open/open/[controller]")]
    [ApiController]
    public class BatchController : JsonController
    {
        [HttpPost]
        public IActionResult Index([FromBody] BatchForm form)
        {
            return Render(null);
        }
    }
}
