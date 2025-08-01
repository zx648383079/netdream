using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Navigation.Entities;
using NetDream.Modules.Navigation.Forms;
using NetDream.Modules.Navigation.Models;
using NetDream.Modules.Navigation.Repositories;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.Navigation
{
    [Route("open/navigation/[controller]")]
    [Authorize]
    [ApiController]
    public class CollectController(CollectRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<CollectGroupModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index()
        {
            return RenderData(repository.All());
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CollectEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] CollectForm form)
        {
            var res = repository.Save(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<CollectGroupModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult BatchSave([FromBody] CollectBatchForm form)
        {
            var res = repository.BatchSave(form);
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

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Clear()
        {
            repository.Clear();
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Reset()
        {
            repository.Reset();
            return RenderData(true);
        }


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CollectGroupEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult GroupSave([FromBody] CollectGroupForm form)
        {
            var res = repository.GroupSave(form);
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
        public IActionResult GroupDelete(int id)
        {
            repository.GroupRemove(id);
            return RenderData(true);
        }
    }
}
