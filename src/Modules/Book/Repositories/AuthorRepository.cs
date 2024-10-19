using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Repositories;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Book.Repositories
{
    public class AuthorRepository(IDatabase db, 
        IClientEnvironment environment,
        IUserRepository userStore) : CRUDRepository<AuthorEntity>(db)
    {
        public Page<AuthorEntity> Search(string keywords = "", int page = 1, params int[] ids)
        {
            var sql = new Sql();
            sql.Select("id", "name", "avatar").From<AuthorEntity>(db);
            if (ids.Length > 0)
            {
                sql.WhereIn("id", ids);
            }
            SearchHelper.Where(sql, "name", keywords);
            return db.Page<AuthorEntity>(page, 20, sql);
        }

        public AuthorModel? Profile(int id)
        {
            return AppendProfile(Get(id));
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
                var sql = new Sql();
                sql.Select("COUNT(id) as count", "SUM(size) as size")
                    .From<BookEntity>(db)
                    .Where("author_id=@0", model.Id);
                var data = db.First<Dictionary<string, int>>(sql);
                author.BookCount = data["count"];
                author.WordCount = data["size"];
            }
            return author;
        }

        public AuthorModel? ProfileByAuth()
        {
            var model = db.First<AuthorEntity>("WHERE user_id=@0", environment.UserId);
            if (model is null)
            {
                var user = userStore.Get(environment.UserId);
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
            var model = db.First<AuthorEntity>("WHERE user_id=@0", environment.UserId);
            if (model is not null && model.Id > 0)
            {
                return model.Id;
            }
            var user = userStore.Get(environment.UserId);
            model = new AuthorEntity()
            {
                Name = user.Name,
                Avatar = user.Avatar,
                UserId = user.Id
            };
            db.TrySave(model);
            return model.Id;
        }
    }
}
