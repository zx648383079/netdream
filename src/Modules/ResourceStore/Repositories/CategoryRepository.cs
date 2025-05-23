using Microsoft.EntityFrameworkCore;
using NetDream.Modules.ResourceStore.Entities;
using NetDream.Modules.ResourceStore.Forms;
using NetDream.Modules.ResourceStore.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.ResourceStore.Repositories
{
    public class CategoryRepository(ResourceContext db)
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
            db.Categories.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Categories.Where(i => i.Id == id).ExecuteDelete();
        }

        public CategoryEntity[] GetChildren(int parent = 0)
        {
            return db.Categories.Where(i => i.ParentId == parent).ToArray();
        }


        public IOperationResult<CategoryModel> GetFull(int id)
        {
            var model = db.Categories.Where(i => i.Id == id).FirstOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<CategoryModel>("id is error");
            }
            var res = model.CopyTo<CategoryModel>();
            res.Children = GetChildren(model.Id);
            return OperationResult.Ok(res);
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

        public ITreeItem[] Tree()
        {
            var data = db.Categories.Select(i => new CategoryTreeItem()
                {
                    Id = i.Id,
                    ParentId = i.ParentId,
                    Name = i.Name,
                }).OrderBy(i => i.ParentId).ToArray();
            return TreeHelper.Create(data);
        }


        public IPage<CategoryEntity> Search(QueryForm form, int[] idItems)
        {
            return db.Categories.Search(form.Keywords, "name")
                .Where(i => idItems.Contains(i.Id))
                .ToPage(form);
        }

        public CategoryModel[] Recommend(string extra = "")
        {
            var items = db.Categories.Where(i => i.IsHot == 1).ToArray()
                .CopyTo<CategoryEntity, CategoryModel>();
            var extraTags = extra.Split(',');
            if (extraTags.Contains("items"))
            {
                foreach (var item in items)
                {
                    item.Items = ResourceRepository.AsList(
                        db.Resources.Where(i => i.CatId == item.Id)
                        .Take(5))
                        .ToArray();
                }
            }
            return items;
        }

        public int[] GetAllChildrenId(int id)
        {
            var data = db.Categories.Select(i => new CategoryEntity()
            {
                Id = i.Id,
                ParentId = i.ParentId,
            }).ToArray();
            return TreeHelper.GetChildren(data, id, true);
        }

        public static void Include(ResourceContext db, IWithCategoryModel[] items)
        {
            var idItems = items.Select(item => item.CatId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Categories.Where(i => idItems.Contains(i.Id))
                .Select(i => new ListLabelItem(i.Id, i.Name))
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
