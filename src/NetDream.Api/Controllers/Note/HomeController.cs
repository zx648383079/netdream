﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Note.Entities;
using NetDream.Modules.Note.Forms;
using NetDream.Modules.Note.Models;
using NetDream.Modules.Note.Repositories;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.Note
{
    [Route("open/note")]
    [ApiController]
    public class HomeController(NoteRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageResponse<NoteListItem>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index([FromQuery] NoteQueryForm form)
        {
            return RenderPage(repository.GetList(form));
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(NoteEntity), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Save([FromBody] NoteForm form)
        {
            var res = repository.SelfSave(form);
            if (res.Succeeded)
            {
                return Render(res.Result);
            }
            return RenderFailure(res.Message);
        }

        [HttpDelete]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(typeof(DataOneResponse<bool>), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Delete(int id)
        {
            repository.SelfRemove(id);
            return RenderData(true);
        }
    }
}
