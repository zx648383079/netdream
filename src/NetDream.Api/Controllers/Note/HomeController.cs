using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Note.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Api.Controllers.Note
{
    [Route("open/note")]
    [ApiController]
    public class HomeController(NoteRepository repository) : JsonController
    {

        public IActionResult Index([FromQuery] QueryForm form, int notice = 0)
        {
            return RenderPage(repository.GetList(form, notice: notice > 0));
        }
    }
}
