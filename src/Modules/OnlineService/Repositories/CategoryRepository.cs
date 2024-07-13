using Modules.OnlineService.Entities;
using NetDream.Modules.OnlineService.Forms;
using NetDream.Modules.OnlineService.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Repositories;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.OnlineService.Repositories
{
    public class CategoryRepository(IDatabase db, IUserRepository userStore): CRUDRepository<CategoryEntity>(db)
    {
        public IList<CategoryEntity> All()
        {
            return db.Fetch<CategoryEntity>();
        }

        private void WithCategory(IEnumerable<IWithCategoryModel> items)
        {
            var idItems = items.Select(item => item.CatId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Fetch<CategoryEntity>($"WHERE id IN({string.Join(',', idItems)})");
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.CatId == it.Id)
                    {
                        item.Category = it;
                        break;
                    }
                }
            }
        }

        public Page<CategoryUserModel> UserList(int category = 0, 
            string keywords = "", int page = 1)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                var res = db.Page<CategoryUserModel>(page, 20, category > 0 ? "cat_id=" + category : string.Empty);
                userStore.WithUser(res.Items);
                WithCategory(res.Items);
                return res;
            }
            var sql = new Sql().Select("user_id").From<CategoryUserEntity>(db);
            if (category > 0)
            {
                sql.Where("cat_id=@0", category);
            }
            var userId = db.Pluck<int>(sql);
            userId = userStore.SearchUserId(keywords, userId, true);
            if (userId.Count == 0)
            {
                return new Page<CategoryUserModel>();
            }
            sql = new Sql().Select("user_id").From<CategoryUserEntity>(db);
            if (category > 0)
            {
                sql.Where("cat_id=@0", category);
            }
            sql.WhereIn("user_id", [..userId]);
            var items = db.Page<CategoryUserModel>(page, 20, sql);
            userStore.WithUser(items.Items);
            WithCategory(items.Items);
            return items;
        }

        public void UserAdd(int category, int[] user)
        {
            var sql = new Sql().Select("user_id").From<CategoryUserEntity>(db)
                .Where("cat_id=@0", category);
            var userIds = db.Pluck<int>(sql);
            var data = new List<CategoryUserEntity>();
            var exit = new List<int>();
            foreach (var item in user)
            {
                if (item < 1 || userIds.Contains(item) || exit.Contains(item))
                {
                    continue;
                }
                data.Add(new CategoryUserEntity()
                {
                    CatId = category,
                    UserId = item,
                });
                exit.Add(item);
            }
            if (data.Count == 0)
            {
                throw new Exception("客服已存在");
            }
            db.InsertBatch(data);
        }

        public void UserRemove(int category, int[] user)
        {

            db.Delete<CategoryUserEntity>($"WHERE cat_id={category} AND user_id IN({string.Join(',', user)})");
        }

        public Page<CategoryWordModel> WordList(int category = 0, 
            string keywords = "", int page = 1)
        {
            var sql = new Sql().Select("*").From<CategoryWordEntity>(db);
            if (category >0)
            {
                sql.Where("cat_id=@0", category);
            }
            SearchHelper.Where(sql, "content", keywords);
            var items = db.Page<CategoryWordModel>(page, 20, sql);
            WithCategory(items.Items);
            return items;
        }

        public CategoryWordEntity WordSave(CategoryWordForm data)
        {
            var model = data.Id > 0 ? db.SingleById<CategoryWordEntity>(data.Id) :
                new CategoryWordEntity();
            model.Content = data.Content;
            model.CatId = data.CatId;
            db.TrySave(model);
            return model;
        }

        public void WordRemove(int id)
        {
            db.DeleteById<CategoryWordEntity>(id);
        }

        public IList<CategoryModel> WordAll()
        {
            var items = db.Fetch<CategoryModel>("ORDER BY id ASC");
            var res = db.Fetch<CategoryWordEntity>();
            foreach (var item in items)
            {
                item.Words = [];
                foreach (var it in res)
                {
                    if (it.CatId == item.Id)
                    {
                        item.Words.Add(it);
                    }
                }
            }
            return items;
        }

        public bool HasService(int userId)
        {
            return db.FindCount<CategoryUserEntity>("user_id=@0", userId) > 0;
        }

        public bool HasWord(int word)
        {
            return db.FindCount<CategoryWordEntity>("id=@0", word) > 0;
        }
    }
}
