using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Forms;
using NetDream.Modules.Book.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Book.Repositories
{
    public class AuthorRepository(BookContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public IPage<AuthorEntity> GetList(QueryForm form)
        {
            return db.Authors.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public IOperationResult<AuthorEntity> Get(int id)
        {
            return OperationResult.OkOrFail(db.Authors.Where(i => i.Id == id).SingleOrDefault());
        }
        public IOperationResult<AuthorEntity> Save(AuthorForm data)
        {
            var model = data.Id > 0 ? db.Authors.Where(i => i.Id == data.Id)
                .Single() :
                new AuthorEntity();
            if (model is null)
            {
                return OperationResult.Fail<AuthorEntity>("id error");
            }
            model.Name = data.Name;
            db.Authors.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Authors.Where(i => i.Id == id).ExecuteDelete();
        }


        public IPage<AuthorEntity> Search(string keywords = "", int page = 1, 
            params int[] ids)
        {
            return db.Authors.Search(keywords, "name")
                .When(ids.Length > 0, i => ids.Contains(i.Id))
                .Select(i => new AuthorEntity()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Avatar = i.Avatar,
                }).ToPage(page);
        }

        public IOperationResult<AuthorModel> Profile(int id)
        {
            var model = db.Authors.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<AuthorModel>.Fail("数据错误");
            }
            return OperationResult.OkOrFail(AppendProfile(model));
        }

        protected AuthorModel? AppendProfile(AuthorEntity? model)
        {
            if (model is null)
            {
                return null;
            }
            var author = new AuthorModel(model);
            if (model.Id > 0)
            {
                var data = db.Books.Where(i => i.AuthorId == model.Id)
                    .GroupBy(i => i.AuthorId)
                    .Select(i => new {
                        Count = i.Count(), 
                        Size = i.Sum(j => j.Size),
                    }).Single();
                author.BookCount = data.Count;
                author.WordCount = data.Size;
            }
            return author;
        }

        public AuthorModel? ProfileByAuth()
        {
            var model = db.Authors.Where(i => i.UserId == client.UserId).Single();
            if (model is null)
            {
                var user = userStore.Get(client.UserId);
                return new AuthorModel() 
                {
                    Name = user.Name,
                    Avatar = user.Avatar,
                };
            }
            return AppendProfile(model);
        }

        public int AuthAuthor()
        {
            var model = db.Authors.Where(i => i.UserId == client.UserId).Single();
            if (model is not null && model.Id > 0)
            {
                return model.Id;
            }
            var user = userStore.Get(client.UserId);
            model = new AuthorEntity()
            {
                Name = user.Name,
                Avatar = user.Avatar,
                UserId = user.Id
            };
            db.Authors.Save(model, client.Now);
            db.SaveChanges();
            return model.Id;
        }
    }
}
