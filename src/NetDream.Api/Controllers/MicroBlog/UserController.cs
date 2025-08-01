using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.MicroBlog.Models;
using NetDream.Modules.MicroBlog.Repositories;
using NetDream.Modules.OpenPlatform;

namespace NetDream.Api.Controllers.MicroBlog
{
    [Route("open/micro/[controller]")]
    [ApiController]
    public class UserController(UserRepository repository) : JsonController
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(UserOpenResult), 200)]
        [ProducesResponseType(typeof(FailureResponse), 404)]
        public IActionResult Index(int id)
        {
            return Render(repository.Get(id));
        }
    }
}
