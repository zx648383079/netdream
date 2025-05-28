using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Backend.Forms;
using NetDream.Modules.Shop.Backend.Models;
using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    public class CategoryRepository(ShopContext db, IClientContext client)
    {
        public IPage<CategoryEntity> GetList(QueryForm form)
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
            model.Position = data.Position;
            model.Banner = data.Banner;
            model.AppBanner = data.AppBanner;
            model.ParentId = data.ParentId;
            db.Categories.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Categories.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }
        public void Refresh()
        {
            db.Categories.RefreshPk((old_id, new_id) => {
                db.Categories.Where(i => i.ParentId == old_id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.ParentId, new_id));
                db.Goods.Where(i => i.CatId == old_id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.CatId, new_id));
            });
        }

        public ITreeItem[] All(bool full = false)
        {
            var data = db.Categories.OrderBy(i => i.Position)
                .Select(i => new CategoryTreeItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    ParentId = i.ParentId
                })
                .ToArray();
            return TreeHelper.Sort(data);
        }

        public IPage<CategoryEntity> Search(QueryForm form, int[] idItems)
        {
            return db.Categories.Search(form.Keywords, "name")
                .When(idItems.Length > 0, i => idItems.Contains(i.Id))
                .OrderBy(i => i.Id)
                .ToPage(form);
        }

        public int FindOrNew(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return 0;
            }
            var id = db.Categories.Where(i => i.Name == name).Value(i => i.Id);
            if (id > 0)
            {
                return id;
            }
            var model = new CategoryEntity()
            {
                Name = name,
            };
            db.Categories.Add(model);
            db.SaveChanges();
            return model.Id;
        }
    }
}
