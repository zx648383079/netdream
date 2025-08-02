using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.UserIdentity.Repositories;

namespace NetDream.Api.Controllers.Exam
{
    [Route("open/exam/admin/[controller]")]
    [Authorize(Roles = IdentityRepository.Administrator)]
    [ApiController]
    public class QuestionBackendController : JsonController
    {
    }
}
