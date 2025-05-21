using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineService.Entities;
using NetDream.Modules.OnlineService.Forms;
using NetDream.Modules.OnlineService.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.OnlineService.Repositories
{
    public class CategoryRepository(OnlineServiceContext db, 
        IUserRepository userStore)
    {
        public IPage<CategoryEntity> GetList(string keywords = "", int page = 1)
        {
            return db.Categories.Search(keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(page);
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

        public CategoryEntity[] All()
        {
            return db.Categories.ToArray();
        }

        public IPage<CategoryUserModel> UserList(int category = 0, 
            string keywords = "", int page = 1)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                var res = db.CategoryUsers.When(category > 0, i=> i.CatId == category).ToPage(page)
                    .CopyTo<CategoryUserEntity, CategoryUserModel>();
                userStore.Include(res.Items);
                return res;
            }
            var userId = db.CategoryUsers.When(category > 0, i => i.CatId == category)
                .Select(i => i.UserId).ToArray();
            userId = userStore.SearchUserId(keywords, userId, true);
            if (userId.Length == 0)
            {
                return new Page<CategoryUserModel>();
            }
            var items = db.CategoryUsers.When(category > 0, i => i.CatId == category)
                .Where(i => userId.Contains(i.UserId)).ToPage(page)
                .CopyTo<CategoryUserEntity, CategoryUserModel>();
            userStore.Include(items.Items);
            return items;
        }

        public IOperationResult UserAdd(int category, int[] user)
        {
            var userIds = db.CategoryUsers.When(category > 0, i => i.CatId == category)
                .Select(i => i.UserId).ToArray();
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
                return OperationResult.Fail("客服已存在");
            }
            db.CategoryUsers.AddRange(data);
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public void UserRemove(int category, int[] user)
        {
            db.CategoryUsers.Where(i => i.CatId == category && user.Contains(i.UserId)).ExecuteDelete();
        }

        public IPage<CategoryWordEntity> WordList(int category = 0, 
            string keywords = "", int page = 1)
        {
            return db.CategoryWords.Include(i => i.Category)
                .Search(keywords, "content")
                .When(category > 0, i => i.CatId == category)
                .ToPage(page);
        }

        public IOperationResult<CategoryWordEntity> WordSave(CategoryWordForm data)
        {
            var model = data.Id > 0 ? db.CategoryWords.Where(i => i.Id == data.Id).Single() :
                new CategoryWordEntity();
            if (model is null)
            {
                return OperationResult.Fail<CategoryWordEntity>("id is error");
            }
            model.Content = data.Content;
            model.CatId = data.CatId;
            db.CategoryWords.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void WordRemove(int id)
        {
            db.CategoryWords.Where(i => i.Id == id).ExecuteDelete();
        }

        public CategoryEntity[] WordAll()
        {
            return db.Categories.Include(i => i.Words)
                .OrderBy(i => i.Id).ToArray();
        }

        public bool HasService(int userId)
        {
            return db.CategoryUsers.Where(i => i.UserId == userId).Any();
        }

        public bool HasWord(int word)
        {
            return db.CategoryWords.Where(i => i.Id == word).Any();
        }
    }
}
