using NetDream.Modules.Auth.Entities;
using NetDream.Modules.UserAccount.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Auth.Repositories
{
    public class LogRepository(AuthContext db)
    {
        public IPage<LoginLogEntity> LoginLog(LogQueryForm form)
        {
            return db.LoginLogs.Search(form.Keywords, "ip")
                .When(form.User > 0, i => i.UserId == form.User)
                .OrderByDescending(i => i.CreatedAt)
                .ToPage(form);
        }
    }
}
