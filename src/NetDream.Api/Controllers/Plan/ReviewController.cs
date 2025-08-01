using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.Plan.Models;
using NetDream.Modules.Plan.Repositories;
using NetDream.Shared.Helpers;
using System;

namespace NetDream.Api.Controllers.Plan
{
    [Route("open/task/[controller]")]
    [Authorize()]
    [ApiController]
    public class ReviewController(ReviewRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<ReviewListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(string type, string date = "", bool ignore = false)
        {
            var now = DateTime.TryParse(date, out var res) ? res : DateTime.Now;
            var (begin, end) = type switch
            {
                "week" => TimeHelper.WeekRange(now),
                "month" => TimeHelper.MonthRange(now),
                _ => TimeHelper.DayRange(now),
            };
            return RenderData(repository.Statistics(begin, end, ignore));
        }
    }
}
