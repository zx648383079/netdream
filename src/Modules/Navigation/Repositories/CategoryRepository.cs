using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Navigation.Entities;
using NetDream.Modules.Navigation.Forms;
using NetDream.Modules.Navigation.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Navigation.Repositories
{
    public class CategoryRepository(NavigationContext db)
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
                .SingleOrDefault() :
                new CategoryEntity();
            if (model is null)
            {
                return OperationResult.Fail<CategoryEntity>("id error");
            }
            model.Name = data.Name;
            model.Icon = data.Icon;
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

        public CategoryTreeItem[] LevelTree(int[] excludes)
        {
            var data = db.Categories.Where(i =>
                !excludes.Contains(i.Id) && !excludes.Contains(i.ParentId))
                .Select(i => new CategoryTreeItem()
                {
                    Id = i.Id,
                    ParentId = i.ParentId,
                    Name = i.Name,
                }).ToArray();
            return TreeHelper.Sort(data);
        }

        internal static void Include(NavigationContext db, IWithCategoryModel[] items)
        {
            var idItems = items.Select(item => item.CatId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Categories.Where(i => idItems.Contains(i.Id))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.CatId > 0 && data.TryGetValue(item.CatId, out var res))
                {
                    item.Category = res;
                }
            }
        }
    }
}
