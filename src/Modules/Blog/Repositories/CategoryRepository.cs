using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Blog.Repositories
{
    public class CategoryRepository(BlogContext db, 
        LocalizeRepository localize)
    {
        private IList<CategoryModel> Boot()
        {
            var data = db.Categories.Select<CategoryEntity, CategoryModel>().ToArray();
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
        public IList<CategoryModel> Get()
        {
            return Boot();
        }
        public CategoryModel? Get(int id)
        {
            foreach (var item in Boot())
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }
        public IList<CategoryModel> LocalizeGet()
        {
            var items = Get();
            foreach (var item in items)
            {
                item.Name = localize.FormatValueWidthPrefix(item, "name");
            }
            return items;
        }
        public CategoryModel? LocalizeGet(int id)
        {
            var data = Get(id);
            if (data is null)
            {
                return data;
            }
            data.Name = localize.FormatValueWidthPrefix(data, "name");
            return data;
        }


        internal static void WithCategory(BlogContext db, IEnumerable<IWithCategoryModel> items)
        {
            var idItems = items.Select(item => item.TermId);
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Categories.Where(i => idItems.Contains(i.Id))
                .Select(i => new CategoryListItem()
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
