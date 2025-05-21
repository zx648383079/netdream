using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Legwork.Entities;
using NetDream.Modules.Legwork.Forms;
using NetDream.Modules.Legwork.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Legwork.Repositories
{
    public class CategoryRepository(LegworkContext db, IUserRepository userStore)
    {
        public IPage<CategoryEntity> GetMangeList(QueryForm form)
        {
            return db.Categories.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public CategoryEntity? Get(int id)
        {
            return db.Categories.Where(i => i.Id == id).Single();
        }
        public IOperationResult<CategoryEntity> Save(CategoryForm data)
        {
            var model = data.Id > 0 ? db.Categories.Where(i => i.Id == data.Id)
                .Single() :
                new CategoryEntity();
            if (model is null)
            {
                return OperationResult.Fail<CategoryEntity>("id error");
            }
            model.Name = data.Name;
            model.Description = data.Description;
            model.Icon = data.Icon;
            db.Categories.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Categories.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public IPage<ProviderListItem> ProviderList(int id, QueryForm form, int status = 0)
        {
            var links = db.CategoryProvider
                .Where(i => i.CatId == id)
                .When(status > 0, i => i.Status == 1)
                .Select(i => new KeyValuePair<int, byte>(i.UserId, i.Status)).ToDictionary();
            if (links.Count == 0)
            {
                return new Page<ProviderListItem>(0, form);
            }
            var data = userStore.Search(form, links.Keys.ToArray(), false);

            return new Page<ProviderListItem>(data)
            {
                Items = data.Items.Select(i => new ProviderListItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Avatar = i.Avatar,
                    IsOnline = i.IsOnline,
                    Status = links[i.Id]
                }).ToArray()
            };
        }

        public ListLabelItem[] All()
        {
            return db.Categories.Select(i => new ListLabelItem(i.Id, i.Name)).ToArray();
        }
    }
}
