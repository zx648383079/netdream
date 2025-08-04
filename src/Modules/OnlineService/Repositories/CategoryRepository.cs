using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineService.Entities;
using NetDream.Modules.OnlineService.Forms;
using NetDream.Modules.OnlineService.Models;
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
        public IPage<CategoryEntity> GetList(QueryForm form)
        {
            return db.Categories.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public IOperationResult<CategoryEntity> Get(int id)
        {
            var model = db.Categories.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<CategoryEntity>("id error");
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

        public CategoryEntity[] All()
        {
            return db.Categories.ToArray();
        }

        public IPage<CategoryUserListItem> UserList(UserQueryForm form)
        {
            if (string.IsNullOrWhiteSpace(form.Keywords))
            {
                var res = db.CategoryUsers.When(form.Category > 0, i => i.CatId == form.Category)
                    .ToPage(form, i => i.SelectAs());
                userStore.Include(res.Items);
                return res;
            }
            var userId = db.CategoryUsers.When(form.Category > 0, i => i.CatId == form.Category)
                .Select(i => i.UserId).ToArray();
            userId = userStore.SearchUserId(form.Keywords, userId, true);
            if (userId.Length == 0)
            {
                return new Page<CategoryUserListItem>();
            }
            var items = db.CategoryUsers.When(form.Category > 0, i => i.CatId == form.Category)
                .Where(i => userId.Contains(i.UserId))
                .ToPage(form, i => i.SelectAs());
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
            db.SaveChanges();
        }

        public IPage<CategoryWordEntity> WordList(UserQueryForm form)
        {
            return db.CategoryWords.Include(i => i.Category)
                .Search(form.Keywords, "content")
                .When(form.Category > 0, i => i.CatId == form.Category)
                .ToPage(form);
        }

        public IOperationResult<CategoryWordEntity> WordSave(CategoryWordForm data)
        {
            var model = data.Id > 0 ? db.CategoryWords.Where(i => i.Id == data.Id).SingleOrDefault() :
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
            db.SaveChanges();
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
