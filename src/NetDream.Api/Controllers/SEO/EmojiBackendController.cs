using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.SEO.Entities;
using NetDream.Modules.SEO.Forms;
using NetDream.Modules.SEO.Models;
using NetDream.Modules.SEO.Repositories;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.SEO
{
    [Route("open/seo/admin/emoji")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class EmojiBackendController(EmojiRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<EmojiEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] EmojiQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(EmojiEntity), 200)]
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
        [ProducesResponseType(typeof(EmojiEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] EmojiForm form)
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

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataResponse<EmojiCategoryEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Category(string keywords = "")
        {
            return RenderData(repository.CatList(keywords));
        }


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(EmojiEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult CategorySave([FromBody] EmojiCategoryForm form)
        {
            var res = repository.SaveCategory(form);
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
        public IActionResult CategoryDelete(int id)
        {
            repository.RemoveCategory(id);
            return RenderData(true);
        }
    }
}
