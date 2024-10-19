using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Forms;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NPoco;

namespace NetDream.Modules.Book.Repositories
{
    public class RoleRepository(IDatabase db)
    {
        public Page<RoleEntity> GetList(int book, string keywords = "", int page = 1)
        {
            var sql = new Sql();
            sql.Select().From<RoleEntity>(db)
                .Where("book_id=@0", book);
            SearchHelper.Where(sql, ["name", "character"], keywords);
            return db.Page<RoleEntity>(page, 20, sql);
        }

        public object All(int book)
        {
            var items = db.Fetch<RoleEntity>("WHERE book_id=@0", book);
            if (items.Count == 0)
            {
                return new { items };
            }
            var idItems = items.Select(x => x.Id).ToArray();
            var sql = new Sql();
            sql.Select().From<RoleRelationEntity>(db)
                .WhereIn("role_id", idItems);
            var linkItems = db.Fetch<RoleRelationEntity>(sql);
            return new {  items, link_items = linkItems};
        }


        public RoleEntity Save(RoleForm data)
        {
            var model = data.Id > 0 ? db.SingleById<RoleEntity>(data.Id) : new RoleEntity();
            model.Avatar = data.Avatar;
            model.Name = data.Name;
            model.Description = data.Description;
            model.BookId = data.BookId;
            model.X = data.X;
            model.Y = data.Y;
            if (!db.TrySave(model))
            {
                throw new Exception("error");
            }
            AddLink(model.Id, data.linkId, data.linkTitle ?? string.Empty);
            return model;
        }

        public void Remove(int id)
        {
            var model = db.SingleById<RoleEntity>(id);
            if (model is null)
            {
                throw new Exception("角色不存在");
            }
            db.DeleteWhere<RoleEntity>("id=@0", id);
            db.DeleteWhere<RoleEntity>("role_id=@0 or role_link=@0", id);
        }

        public void AddLink(int from, int to, string title)
        {
            if (from < 1 || to < 1)
            {
                return;
            }
            var model = db.First<RoleRelationEntity>("WHERE role_id=@0 and role_link=@1",
                from, to);
            if (model is not null)
            {
                model.Title = title;
                db.UpdateWhere<RoleRelationEntity>("title=@0", "id=@1", title, model.Id);
            }
            else
            {
                db.Insert(new RoleRelationEntity()
                {
                    RoleId = from,
                    RoleLink = to,
                    Title = title,
                });
            }
        }

        public void RemoveLink(int from, int to)
        {
            if (from < 1 || to < 1)
            {
                return;
            }
            db.DeleteWhere<RoleRelationEntity>("role_id=@0 and role_link=@1", from, to);
        }
    }
}
