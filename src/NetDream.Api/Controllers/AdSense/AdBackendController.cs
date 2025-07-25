using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.AdSense.Entities;
using NetDream.Modules.AdSense.Forms;
using NetDream.Modules.AdSense.Repositories;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.AdSense
{
    [Route("open/ad/admin/ad")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class AdBackendController(AdRepository repository) : JsonController
    {

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<AdEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] AdQueryForm form)
        {
            return RenderPage(repository.ManageList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AdEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.ManageGet(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(AdEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] AdForm form)
        {
            var res = repository.ManageSave(form);
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
            repository.ManageRemove(id);
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<PositionEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult PositionAll()
        {
            return RenderData(repository.PositionAll());
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<PositionEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Position([FromQuery] QueryForm form)
        {
            return RenderPage(repository.ManagePositionList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PositionEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult DetailPosition(int id)
        {
            var res = repository.ManagePosition(id);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PositionEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SavePosition([FromBody] PositionForm form)
        {
            var res = repository.ManagePositionSave(form);
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
        public IActionResult DeletePosition(int id)
        {
            repository.ManagePositionRemove(id);
            return RenderData(true);
        }
    }
}
