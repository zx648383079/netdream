using Microsoft.AspNetCore.Mvc;
using NetDream.Shared.Interfaces;
using NetDream.Modules.Note.Forms;
using NetDream.Modules.Note.Repositories;
using NetDream.Web.Base.Extensions;
using NetDream.Web.Base.Http;

namespace NetDream.Web.Areas.Note.Controllers
{
    [Area("Note")]
    public class HomeController(NoteRepository repository, IClientContext environment) : JsonController
    {
        public IActionResult Index(string keywords = "", int id = 0, int user = 0, int page = 1)
        {
            ViewData["isGuest"] = environment.UserId == 0;
            ViewData["items"] = repository.GetList(keywords, user, id, false, page);
            return View();
        }

        public IActionResult Page(string keywords = "", int id = 0, int user = 0, int page = 1)
        {
            ViewData["isGuest"] = environment.UserId == 0;
            var items = repository.GetList(keywords, user, id, false, page);
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
            repository.SaveSelf(form);
            return RenderData(new {
                Refresh = true
            });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            repository.RemoveSelf(id);
            return RenderData(true);
        }
    }
}
