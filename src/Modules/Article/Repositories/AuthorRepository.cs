using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Article.Entities;
using NetDream.Modules.Article.Forms;
using NetDream.Modules.Article.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Article.Repositories
{
    public class AuthorRepository(ArticleContext db, IUserRepository userStore) : IAuthorRepository
    {
        public IPage<AuthorEntity> AdvancedGet(IQueryForm form)
        {
            return db.Authors.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public IOperationResult<AuthorEntity> AdvancedSave(AuthorForm data)
        {
            var model = data.Id > 0 ? db.Authors.Where(i => i.Id == data.Id)
                .SingleOrDefault() :
                new AuthorEntity();
            if (model is null)
            {
                return OperationResult.Fail<AuthorEntity>("id error");
            }
            model.Name = data.Name;
            model.Avatar = data.Avatar;
            model.Email = data.Email;
            model.Url = data.Url;
            model.Description = data.Description;
            db.Authors.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void AdvancedRemove(int id)
        {
            db.Authors.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }


        public IPage<CategoryEntity> Search(IQueryForm form, int[] idItems)
        {
            return db.Categories.Search(form.Keywords, "name")
                .When(idItems?.Length > 0, i => idItems.Contains(i.Id))
                .ToPage(form);
        }
        public IOperationResult Create(int user)
        {
            if (Exist(user))
            {
                return OperationResult.Ok();
            }
            var data = userStore.Get(user);
            if (data == null) 
            {
                return OperationResult.Fail("用户不存在");
            }
            db.Authors.Save(new AuthorEntity()
            {
                UserId = user,
                Name = data.Name,
                Avatar = data.Avatar,
            });
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public bool Exist(int user)
        {
            return db.Authors.Where(i => i.UserId == user).Any();
        }

        public IOperationResult<IUser> From(int user)
        {
            var model = db.Authors.Where(i => i.UserId == user).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<IUser>("not found");
            }
            return OperationResult.Ok<IUser>(new AuthorLabelItem(model));
        }

        public IOperationResult<IUser> Get(int id)
        {
            var model = db.Authors.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<IUser>("not found");
            }
            return OperationResult.Ok<IUser>(new AuthorLabelItem(model));
        }

        public void Include(IEnumerable<IWithUserModel> items)
        {
            var idItems = items.Select(item => item.UserId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Authors.Where(i => idItems.Contains(i.UserId)).SelectAsLabel()
                .ToDictionary(i => i.UserId);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.UserId > 0 && data.TryGetValue(item.UserId, out var user))
                {
                    item.User = user;
                }
            }
        }
    }
}
