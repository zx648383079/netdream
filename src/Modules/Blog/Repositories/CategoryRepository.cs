using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Repositories;
using NPoco;

namespace NetDream.Modules.Book.Repositories
{
    public class CategoryRepository(IDatabase db, 
        LocalizeRepository localize)
    {
        private IList<CategoryModel> Boot()
        {
            var data = db.Fetch<CategoryModel>();
            var sql = new Sql();
            sql.Select("term_id", "COUNT(*) as count")
                .From<BlogEntity>(db)
                .Where("parent_id=0")
                .GroupBy("term_id");
            var countItems = db.Dictionary<int, int>(sql);
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

    }
}
