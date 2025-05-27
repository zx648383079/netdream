using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Modules.OnlineMedia.Forms;
using NetDream.Modules.OnlineMedia.Importers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.IO;
using System.Linq;

namespace NetDream.Modules.OnlineMedia.Repositories
{
    public class LiveRepository(MediaContext db, IClientContext client)
    {
        public IPage<LiveEntity> GetList(QueryForm form)
        {
            return db.Lives.Search(form.Keywords, "Name").ToPage(form);
        }

        public IOperationResult<LiveEntity> Get(int id)
        {
            var model = db.Lives.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<LiveEntity>.Fail("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<LiveEntity> Save(LiveForm data)
        {
            if (db.Lives.Where(i => i.Title == data.Title && i.Id != data.Id).Any())
            {
                return OperationResult.Fail<LiveEntity>("title is error");
            }
            var model = data.Id > 0 ? db.Lives.Where(i => i.Id == data.Id)
                 .Single() :
                 new LiveEntity();
            if (model is null)
            {
                return OperationResult.Fail<LiveEntity>("id error");
            }
            model.Title = data.Title;
            model.Thumb = data.Thumb;
            model.Source = data.Source;
            db.Lives.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Lives.Where(i => i.Id == id).ExecuteDelete();
        }

        public IOperationResult Import(Stream input, string fileName)
        {

            var items = new IImporter<LiveEntity>[]
             {
                new DplImporter(db),
                new M3uImporter(db),
                new TxtImporter(db),
             };
            foreach (var importer in items)
            {
                if (!importer.IsMatch(input, fileName))
                {
                    continue;
                }
                foreach (var item in importer.Read(input))
                {
                    CreateIfNot(item);
                }
                db.SaveChanges();
                return OperationResult.Ok();
            }
            return OperationResult.Fail("不支持");
        }

        private void CreateIfNot(LiveEntity data)
        {
            if (db.Lives.Where(i => i.Title == data.Title && i.Id != data.Id).Any())
            {
                return;
            }
            db.Lives.Add(data);
        }

        public IExporter Export()
        {
            return new DplImporter(db);
        }

        public LiveEntity[] GetActiveList()
        {
            return db.Lives.Where(i => i.Status == 1).ToArray();
        }

    }
}
