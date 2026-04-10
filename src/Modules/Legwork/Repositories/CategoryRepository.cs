using NetDream.Modules.Legwork.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Legwork.Repositories
{
    public class CategoryRepository(LegworkContext db, 
        IUserRepository userStore)
    {

        public IPage<UserListItem> ProviderList(int id, QueryForm form, int status = 0)
        {
            var links = db.CategoryProvider
                .Where(i => i.CatId == id)
                .When(status > 0, i => i.Status == 1)
                .Select(i => new KeyValuePair<int, byte>(i.UserId, i.Status)).ToDictionary();
            if (links.Count == 0)
            {
                return new Page<UserListItem>(0, form);
            }
            var data = userStore.Search(form, links.Keys.ToArray(), false);

            return new Page<UserListItem>(data)
            {
                Items = data.Items.Select(i => new UserListItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Avatar = i.Avatar,
                    IsOnline = i.IsOnline,
                    Status = links[i.Id]
                }).ToArray()
            };
        }

    }
}
