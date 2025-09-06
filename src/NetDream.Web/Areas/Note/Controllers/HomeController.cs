using Microsoft.AspNetCore.Mvc;
using NetDream.Shared.Interfaces;
using NetDream.Modules.Note.Forms;
using NetDream.Modules.Note.Repositories;
using NetDream.Web.Base.Extensions;
using NetDream.Web.Base.Http;
using NetDream.Shared.Models;

namespace NetDream.Web.Areas.Note.Controllers
{
    [Area("Note")]
    public class HomeController(NoteRepository repository, IClientContext environment) : JsonController
    {
        public IActionResult Index([FromQuery] NoteQueryForm form)
        {
            ViewData["isGuest"] = environment.UserId == 0;
            ViewData["items"] = repository.GetList(form);
            return View();
        }

        public IActionResult Page([FromQuery] NoteQueryForm form)
        {
            ViewData["isGuest"] = environment.UserId == 0;
            var items = repository.GetList(form);
            ViewData["items"] = items;
            return RenderData(new {
                Html = View().ToHtml(HttpContext),
                has_more = items.CurrentPage < items.TotalPages,
            });
        }

        [HttpPost]
        public IActionResult Save([Bind] NoteForm form)
        {
            if (!ModelState.IsValid)
            {
                return RenderFailure(ModelState);
            }
            repository.SelfSave(form);
            return RenderData(new {
                Refresh = true
            });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            repository.SelfRemove(id);
            return RenderData(true);
        }
    }
}
