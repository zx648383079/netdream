using Microsoft.AspNetCore.Http;
using NetDream.Areas.Open.Entities;

namespace NetDream.Areas.Open.Models
{
    public class PlatformModel: PlatformEntity
    {

        public bool VerifyRule(string path)
        {
            return true;
        }

        public bool VerifyRest(HttpContext context)
        {
            return true;
        }
    }
}
