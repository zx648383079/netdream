using NetDream.Modules.Counter.Entities;
using NetDream.Modules.Counter.Forms;
using NetDream.Modules.Counter.Importers;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Counter.Repositories
{
    public class AnalysisRepository(CounterContext db, IClientContext client)
    {

        public IPage<LogEntity> LogList(LogQueryForm form)
        {
            var startAt = string.IsNullOrWhiteSpace(form.StartAt) ? 0 : TimeHelper.TimestampFrom(form.StartAt);
            var endAt = string.IsNullOrWhiteSpace(form.EndAt) ? 0 : TimeHelper.TimestampFrom(form.EndAt);
            var jumpTo = string.IsNullOrWhiteSpace(form.Goto) ? 0 : TimeHelper.TimestampFrom(form.Goto);
            if (jumpTo > 0)
            {
                var count = db.Logs.Search(form.Keywords, "queries", "pathname")
                    .When(form.UserAgent, i => i.UserAgent == form.UserAgent)
                    .When(form.Hostname, i => i.Hostname == form.Hostname)
                    .When(form.Pathname, i => i.Pathname == form.Pathname)
                    .When(endAt > startAt, i => i.CreatedAt < endAt)
                    .Where(i => i.CreatedAt >= jumpTo)
                    .Count();
                form.Page = (int)Math.Ceiling((float)count / form.PerPage);
            }
            return db.Logs.Search(form.Keywords, "queries", "pathname")
                .When(form.UserAgent, i => i.UserAgent == form.UserAgent)
                .When(form.Hostname, i => i.Hostname == form.Hostname)
                .When(form.Pathname, i => i.Pathname == form.Pathname)
                .When(startAt > 0, i => i.CreatedAt >= startAt)
                .When(endAt > startAt, i => i.CreatedAt < endAt)
                .OrderByDescending(i => i.CreatedAt)
                .ToPage(form);
        }

        public IOperationResult<int> LogImport(LogImportForm data, IUploadFile file)
        {
            ILogImporter importer = data.Engine?.ToLower() switch
            {
                "iis" => new IISLogImporter(),
                "apache" => new ApacheLogImporter(),
                _ => new NginxLogImporter()
            };
            if (string.IsNullOrWhiteSpace(data.Hostname))
            {
                data.Hostname = file.Name;
            }
            var fieldNames = importer.ParseField(data.FieldNames);
            var lastAt = db.Logs.Where(i => i.Hostname == data.Hostname)
                .Max(i => i.CreatedAt);
            using var fs = file.OpenRead();
            var count = 0;
            foreach (var item in importer.Read(fieldNames, fs))
            {
                if (item.CreatedAt <= lastAt)
                {
                    continue;
                }
                if (string.IsNullOrWhiteSpace(item.Hostname))
                {
                    item.Hostname = data.Hostname;
                }
                db.Logs.Add(item);
                count++;
            }
            db.SaveChanges();
            return OperationResult.Ok(count);
        }

        public IOperationResult<AnalysisFlagEntity> Mark(AnalysisMarkForm data)
        {
            var model = db.AnalysisFlags.Where(i => i.UserId == client.UserId && i.ItemType == data.Type && i.ItemValue == data.Value)
                .FirstOrDefault();
            if (model is not null)
            {
                return OperationResult.Ok(model);
            }
            model = new AnalysisFlagEntity()
            {
                ItemType = (byte)data.Type,
                ItemValue = data.Value,
                UserId = client.UserId,
                CreatedAt = client.Now
            };
            db.AnalysisFlags.Add(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }
    }
}
