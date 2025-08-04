using NetDream.Modules.UserAccount.Forms;
using NetDream.Modules.UserAccount.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.UserAccount.Repositories
{
    public class AccountRepository(UserContext db, IUserRepository repository)
    {
        public IPage<AccountLogListItem> LogList(LogQueryForm form)
        {
            var items = db.AccountLogs.Search(form.Keywords, "remark")
                .When(form.User > 0, i => i.UserId == form.User)
                .When(form.Type > 0, i => i.Type == form.Type)
                .ToPage(form, i => i.SelectAs());
            repository.Include(items.Items);
            return items;
        }
    }
}
