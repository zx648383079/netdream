using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.SEO.Entities;
using NetDream.Modules.SEO.Forms;
using NetDream.Modules.SEO.Models;
using NetDream.Modules.SEO.Repositories;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Api.Controllers.SEO
{
    [Route("open/seo/admin/setting")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class SettingBackendController(OptionRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(DataResponse<OptionTreeModel>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index()
        {
            return RenderData(repository.GetEditList());
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AgreementModel), 200)]
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
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] OptionBatchForm data)
        {
            if (data.Field is not null)
            {
                repository.SaveNewOption(data.Field);
            }
            if (data.Option is null)
            {
                return RenderData(true);
            }
            var res = repository.SaveOption(data.Option);
            if (!res.Succeeded)
            {
                return RenderFailure(res.Message);
            }
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(AgreementEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Update([FromBody] OptionForm form)
        {
            var res = repository.Update(form.Id, form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(AgreementEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult SaveOption([FromBody] OptionForm form)
        {
            var res = form.Id > 0 ? repository.Update(form.Id, form) : repository.SaveNewOption(form);
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
