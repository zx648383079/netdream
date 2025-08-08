using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.UserIdentity.Entities;
using NetDream.Modules.UserIdentity.Forms;
using NetDream.Modules.UserIdentity.Models;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Auth
{
    [Route("open/auth/admin")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class RoleBackendController(RoleRepository repository) : JsonController
    {
        [HttpGet]
        [Route("[controller]")]
        [ProducesResponseType(typeof(PageResponse<RoleEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] QueryForm form)
        {
            return RenderData(repository.RoleList(form));
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [ProducesResponseType(typeof(RoleModel), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Detail(int id)
        {
            var res = repository.RoleGet(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        [ProducesResponseType(typeof(RoleEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] RoleForm form)
        {
            var res = repository.RoleSave(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpDelete]
        [Route("[controller]/[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.RoleRemove(id);
            return RenderData(true);
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [ProducesResponseType(typeof(DataResponse<RoleEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult All()
        {
            return RenderData(repository.RoleAll());
        }

        [HttpGet]
        [Route("permission")]
        [ProducesResponseType(typeof(PageResponse<PermissionEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Permission([FromQuery] QueryForm form)
        {
            return RenderData(repository.PermissionList(form));
        }

        [HttpGet]
        [Route("permission/detail")]
        [ProducesResponseType(typeof(PermissionEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult PermissionDetail(int id)
        {
            var res = repository.PermissionGet(id);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return Render(res.Result);
        }

        [HttpPost]
        [Route("permission/save")]
        [ProducesResponseType(typeof(PermissionEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult PermissionSave([FromBody] PermissionForm form)
        {
            var res = repository.PermissionSave(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpDelete]
        [Route("permission/delete")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult PermissionDelete(int id)
        {
            repository.PermissionRemove(id);
            return RenderData(true);
        }

        [HttpGet]
        [Route("permission/all")]
        [ProducesResponseType(typeof(DataResponse<PermissionEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult PermissionAll()
        {
            return RenderData(repository.PermissionAll());
        }
    }
}
