using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineDisk.Entities;
using NetDream.Modules.OnlineDisk.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.OnlineDisk.Repositories
{
    public class TrashRepository(OnlineDiskContext db, IClientContext client)
    {
        public IPage<DiskListItem> GetList(QueryForm form)
        {
            var items = db.Disks.Where(i => i.UserId == client.UserId && i.DeletedAt > 0)
                .OrderByDescending(i => i.DeletedAt)
                .ThenBy(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            DiskRepository.IncludeFile(db, items.Items);
            return items;
        }

        public void Reset(params int[] idItems)
        {
            var items = db.Disks.Where(i =>
                i.UserId == client.UserId && idItems.Contains(i.Id) && i.DeletedAt > 0)
                .ToArray();
            foreach (var item in items)
            {
                Reset(item);
            }
        }

        private void Reset(DiskEntity model)
        {
            db.Disks.Where(i => i.UserId == model.UserId && i.LeftId >= model.LeftId && i.RightId <= model.RightId)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.DeletedAt, 0));
            db.SaveChanges();
        }

        public void Remove(params int[] idItems)
        {
            if (idItems.Length == 0)
            {
                return;
            }
            foreach (var id in idItems)
            {
                var item = db.Disks.Where(i =>
                i.UserId == client.UserId && i.Id == id && i.DeletedAt > 0).SingleOrDefault();
                if (item is null)
                {
                    continue;
                }
                Remove(item);
            }
        }

        private void Remove(DiskEntity model)
        {
            db.Disks.Where(i => i.UserId == model.UserId && i.LeftId >= model.LeftId && i.RightId <= model.RightId)
                .ExecuteDelete();
            var num = model.LeftId - model.RightId - 1;
            db.Disks.Where(i => i.UserId == model.UserId && i.RightId > model.RightId)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.RightId, i => i.RightId + num));
            db.Disks.Where(i => i.UserId == model.UserId && i.LeftId > model.RightId)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.LeftId, i => i.LeftId + num));
            db.SaveChanges();
        }

        public void Clear()
        {
            db.Disks.Where(i => i.DeletedAt > 0 && i.UserId == client.UserId).ExecuteDelete();
            db.SaveChanges();
        }
    }
}
