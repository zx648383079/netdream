﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Finance.Entities;
using NetDream.Modules.Finance.Forms;
using NetDream.Modules.Finance.Models;
using NetDream.Modules.Finance.Repositories;
using NetDream.Modules.OpenPlatform.Models;
using System.IO;

namespace NetDream.Api.Controllers.Finance
{
    [Route("open/finance/[controller]")]
    [Authorize()]
    [ApiController]
    public class LogController(LogRepository repository) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<LogListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] LogQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(LogEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.Get(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(LogEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] LogForm form)
        {
            var res = repository.Save(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.Remove(id);
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Batch([FromBody] BatchLogForm form)
        {
            var res = repository.BatchEdit(form);
            return RenderData(true, $"更新 {res} 条数据");
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Import(IFormFile file)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            var res = repository.Import(ms);
            if (res.Succeeded)
            {
                return RenderData(true);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Export()
        {
            var ms = new MemoryStream();
            repository.Export(ms);
            return new FileStreamResult(ms, "application/vnd.ms-excel")
            {
                FileDownloadName = "流水记录.xls"
            };
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Day([FromBody] DayLogForm form)
        {
            var res = repository.SaveDay(form);
            if (res.Succeeded)
            {
                return RenderData(true);
            }
            return RenderFailure(res.Message);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<int>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Count([FromQuery] LogQueryForm form)
        {
            return RenderData(repository.Count(form));
        }
    }
}
