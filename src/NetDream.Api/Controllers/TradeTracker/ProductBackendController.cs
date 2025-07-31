using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.TradeTracker.Entities;
using NetDream.Modules.TradeTracker.Forms;
using NetDream.Modules.TradeTracker.Models;
using NetDream.Modules.TradeTracker.Repositories;
using NetDream.Modules.UserIdentity.Repositories;
using NetDream.Shared.Models;
using System.IO;

namespace NetDream.Api.Controllers.TradeTracker
{
    [Route("open/tracker/admin/product")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class ProductBackendController(ManagerRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<ProductListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] ProductQueryForm form)
        {
            return RenderPage(repository.GetProductList(form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ProductEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] ProductForm form)
        {
            var res = repository.ProductSave(form);
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
            repository.ProductRemove(id);
            return RenderData(true);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PageResponse<ChannelEntity>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Channel([FromQuery] QueryForm form)
        {
            return RenderPage(repository.ChannelList(form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ChannelEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult ChannelSave([FromBody] ChannelForm form)
        {
            var res = repository.ChannelSave(form);
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
        public IActionResult ChannelDelete(int id)
        {
            repository.ChannelRemove(id);
            return RenderData(true);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Import(IFormFile file)
        {
            if (!file.ContentType.Contains("zip"))
            {
                return RenderFailure("文件不支持，仅支持 zip 压缩文件");
            }
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            ms.Position = 0;
            var res = repository.ProductImport(ms);
            if (res.Succeeded)
            {
                return RenderData(true);
            }
            return RenderFailure(res.Message);
        }
    }
}
