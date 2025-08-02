using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Forms;
using NetDream.Modules.Blog.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Repositories;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetDream.Modules.Blog.Repositories
{
    public class CategoryRepository(BlogContext db, 
        LocalizeRepository localize)
    {
        private IList<CategoryLabelItem> Boot()
        {
            var data = db.Categories.Select<CategoryEntity, CategoryLabelItem>().ToArray();
            var countItems = db.Blogs.Where(i => i.ParentId == 0)
                .GroupBy(i => i.TermId)
                .Select(i => new { TermId = i.Key, Count = i.Count()})
                .ToDictionary(i => i.TermId, i => i.Count);
            foreach (var item in data)
            {
                if (countItems.TryGetValue(item.Id, out int value))
                {
                    item.BlogCount = value;
                }
            }
            return data;
        }
        public IList<CategoryLabelItem> Get()
        {
            return Boot();
        }
        public IOperationResult<CategoryEntity> Get(int id)
        {
            var model = db.Categories.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<CategoryEntity>.Fail("数据错误");
            }
            return OperationResult.Ok(model);
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
            db.Categories.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Categories.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }
        public IList<CategoryLabelItem> LocalizeGet()
        {
            var items = Get();
            foreach (var item in items)
            {
                item.Name = localize.FormatValueWidthPrefix(item, "name");
            }
            return items;
        }
        public IOperationResult<CategoryLabelItem> LocalizeGet(int id)
        {
            var model = db.Categories.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<CategoryLabelItem>.Fail("数据错误");
            }
            var blogCount = db.Blogs.Where(i => i.TermId == model.Id).Count();
            return OperationResult.Ok(new CategoryLabelItem()
            {
                Id = model.Id,
                Name = localize.FormatValueWidthPrefix(model, "name"),
                BlogCount = blogCount
            });
        }


        internal static void Include(BlogContext db, IEnumerable<IWithCategoryModel> items)
        {
            var idItems = items.Select(item => item.TermId);
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Categories.Where(i => idItems.Contains(i.Id))
                .Select(i => new CategoryLabelItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                }).ToArray();
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.TermId == it.Id)
                    {
                        item.Term = it;
                        break;
                    }
                }
            }
        }
    }
}
