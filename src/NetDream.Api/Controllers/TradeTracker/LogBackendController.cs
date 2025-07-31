using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Modules.TradeTracker.Forms;
using NetDream.Modules.TradeTracker.Models;
using NetDream.Modules.TradeTracker.Repositories;
using NetDream.Modules.UserIdentity.Repositories;
using System.IO;

namespace NetDream.Api.Controllers.TradeTracker
{
    [Route("open/tracker/admin/log")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class LogBackendController(ManagerRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<LogListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] LogQueryForm form)
        {
            return RenderPage(repository.LogList(form));
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Add([FromBody] CrawlItemForm form)
        {
            var res = repository.LogAdd([form]);
            if (res.Succeeded)
            {
                return RenderData(true);
            }
            return RenderFailure(res.Message);
        }

        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int[] id)
        {
            repository.LogRemove(id);
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
            var res = repository.LogImport(ms);
            if (res.Succeeded)
            {
                return RenderData(true);
            }
            return RenderFailure(res.Message);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Crawl([FromBody] CrawlForm form)
        {
            var res = repository.CrawlSave(form);
            if (res.Succeeded)
            {
                return RenderData(true);
            }
            return RenderFailure(res.Message);
        }
    }
}
