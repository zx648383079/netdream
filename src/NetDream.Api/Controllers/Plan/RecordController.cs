using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.Plan.Models;
using NetDream.Modules.Plan.Repositories;
using NetDream.Shared.Helpers;
using NetDream.Shared.Models;
using System;

namespace NetDream.Api.Controllers.Plan
{
    [Route("open/task/[controller]")]
    [Authorize()]
    [ApiController]
    public class RecordController(ReviewRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<LogListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form, string type, string date = "")
        {
            var now = DateTime.TryParse(date, out var res) ? res : DateTime.Now;
            var (begin, end) = type switch
            {
                "week" => TimeHelper.WeekRange(now),
                "month" => TimeHelper.MonthRange(now),
                _ => TimeHelper.DayRange(now),
            };
            return RenderPage(repository.LogList(form, TimeHelper.TimestampFrom(begin), TimeHelper.TimestampFrom(end)));
        }
    }
}
