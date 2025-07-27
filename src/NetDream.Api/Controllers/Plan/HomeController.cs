using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.Plan.Entities;
using NetDream.Modules.Plan.Forms;
using NetDream.Modules.Plan.Models;
using NetDream.Modules.Plan.Repositories;
using NetDream.Shared.Models;
using System;

namespace NetDream.Api.Controllers.Plan
{
    [Route("open/task")]
    [Authorize()]
    [ApiController]
    public class HomeController(TaskRepository repository, DayRepository dayStore) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<TaskListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] TaskQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(TaskModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.Detail(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(TaskEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] TaskForm form, int status = -1)
        {
            var res = repository.Save(form, status);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(TaskEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult FastCreate([FromBody] TaskForm form)
        {
            var res = repository.Save(form);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            var data = dayStore.Save(new DayForm()
            {
                TaskId = res.Result.Id
            });
            if (!data.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(data.Result);
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            var res = repository.Remove(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<DayListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Today([FromQuery] QueryForm form, string time = "")
        {
            if (string.IsNullOrWhiteSpace(time))
            {
                time = DateTime.Now.ToString("yyyy-MM-dd");
            }
            return RenderPage(dayStore.GetList(time, form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DayModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult DetailDay(int id)
        {
            var res = dayStore.Detail(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DayEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SaveDay([FromBody] DayForm form)
        {
            var res = dayStore.Save(form);
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
        public IActionResult DeleteDay(int id)
        {
            var res = dayStore.Remove(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DayEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Play(int id, int childId = 0)
        {
            var res = repository.Start(id, childId);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DayEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Pause(int id)
        {
            var res = repository.Pause(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DayEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Stop(int id)
        {
            var res = repository.Stop(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DayEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Check(int id)
        {
            var res = repository.Check(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult BatchAdd(int[] id)
        {
            var res = dayStore.BatchAdd(id);
            if (res.Succeeded)
            {
                return RenderData(true);
            }
            return RenderFailure(res.Message);
        }
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(TaskEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult StopTask(int id)
        {
            var res = repository.StopTask(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult BatchStopTask(int[] id)
        {
            foreach (var item in id)
            {
                if (item < 1)
                {
                    continue;
                }
                var res = repository.StopTask(item);
                if (!res.Succeeded)
                {
                    return RenderFailure(res.Message);
                }
            }
            return RenderData(true);
        }
    }
}
