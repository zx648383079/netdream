using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.Plan.Entities;
using NetDream.Modules.Plan.Forms;
using NetDream.Modules.Plan.Models;
using NetDream.Modules.Plan.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Plan
{
    [Route("open/task/[controller]")]
    [Authorize()]
    [ApiController]
    public class PlanController(PlanRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<PlanListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form, byte type)
        {
            return RenderData(repository.GetList(form, type));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PlanEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] PlanForm form)
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
    }
}
