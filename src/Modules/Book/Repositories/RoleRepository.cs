using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Forms;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Book.Repositories
{
    public class RoleRepository(BookContext db)
    {
        public IPage<RoleEntity> GetList(int book, string keywords = "", int page = 1)
        {
            return db.Roles.Where(i => i.BookId == book).Search(keywords, "name", "character")
                .ToPage(page);
        }

        public object All(int book)
        {
            var items = db.Roles.Where(i => i.BookId == book).ToArray();
            if (items.Length == 0)
            {
                return new { items };
            }
            var idItems = items.Select(x => x.Id).ToArray();
            var linkItems = db.RoleRelations.Where(i => idItems.Contains(i.RoleId))
                .ToArray();
            return new {  items, link_items = linkItems};
        }


        public IOperationResult<RoleEntity> Save(RoleForm data)
        {
            var model = data.Id > 0 ? db.Roles.Where(i => i.Id == data.Id).Single() : new RoleEntity();
            if (model is null)
            {
                return OperationResult<RoleEntity>.Fail("id is error");
            }
            model.Avatar = data.Avatar;
            model.Name = data.Name;
            model.Description = data.Description;
            model.BookId = data.BookId;
            model.X = data.X;
            model.Y = data.Y;
            db.Roles.Add(model);
            if (db.SaveChanges() == 0)
            {
                return OperationResult<RoleEntity>.Fail("error");
            }
            AddLink(model.Id, data.linkId, data.linkTitle ?? string.Empty);
            return OperationResult.Ok(model);
        }

        public IOperationResult Remove(int id)
        {
            var model = db.Roles.Where(i => i.Id == id).Single();
            if (model is null)
            {
                return OperationResult.Fail("角色不存在");
            }
            db.Roles.Where(i => i.Id == id).ExecuteDelete();
            db.RoleRelations.Where(i => i.RoleId == id || i.RoleLink == id).ExecuteDelete();
            return OperationResult.Ok();
        }

        public void AddLink(int from, int to, string title)
        {
            if (from < 1 || to < 1)
            {
                return;
            }
            var model = db.RoleRelations.Where(i => i.RoleId == from && i.RoleLink == to).Single();
            if (model is not null)
            {
                model.Title = title;
                db.RoleRelations.Update(model);
            }
            else
            {
                db.RoleRelations.Add(new RoleRelationEntity()
                {
                    RoleId = from,
                    RoleLink = to,
                    Title = title,
                });
            }
            db.SaveChanges();
        }

        public void RemoveLink(int from, int to)
        {
            if (from < 1 || to < 1)
            {
                return;
            }
            db.RoleRelations.Where(i => i.RoleId == from && i.RoleLink == to).ExecuteDelete();
        }
    }
}
