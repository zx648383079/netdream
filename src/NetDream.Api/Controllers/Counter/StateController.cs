using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;

namespace NetDream.Api.Controllers.Counter
{
    [Route("open/counter/[controller]")]
    [ApiController]
    public class StateController : JsonController
    {
    }
}
