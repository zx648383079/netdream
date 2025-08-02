using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;

namespace NetDream.Api.Controllers.Exam
{
    [Route("open/exam/member/[controller]")]
    [Authorize]
    [ApiController]
    public class PageMemberController : JsonController
    {
    }
}
