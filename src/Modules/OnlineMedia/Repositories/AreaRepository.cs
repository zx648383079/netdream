using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Modules.OnlineMedia.Forms;
using NetDream.Modules.OnlineMedia.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.OnlineMedia.Repositories
{
    public class AreaRepository(MediaContext db)
    {
        public IPage<AreaEntity> GetList(QueryForm form)
        {
            return db.Areas.Search(form.Keywords, "Name").ToPage(form);
        }

        public IOperationResult<AreaEntity> Get(int id)
        {
            var model = db.Areas.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<AreaEntity>.Fail("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<AreaEntity> Save(AreaForm data)
        {
            var model = data.Id > 0 ? db.Areas.Where(i => i.Id == data.Id)
                 .Single() :
                 new AreaEntity();
            if (model is null)
            {
                return OperationResult.Fail<AreaEntity>("id error");
            }
            model.Name = data.Name;
            db.Areas.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Areas.Where(i => i.Id == id).ExecuteDelete();
        }

        public AreaEntity[] All()
        {
            return db.Areas.ToArray();
        }

        internal static void Include(MediaContext db, IWithAreaModel[] items)
        {
            var idItems = items.Select(item => item.AreaId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Areas.Where(i => idItems.Contains(i.Id))
                .Select(i => new ListLabelItem(i.Id, i.Name))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.AreaId > 0 && data.TryGetValue(item.AreaId, out var res))
                {
                    item.Area = res;
                }
            }
        }
    }
}
