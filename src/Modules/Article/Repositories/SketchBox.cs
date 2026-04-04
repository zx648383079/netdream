using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Article.Entities;
using NetDream.Shared.Interfaces;
using System.Linq;
using System.Text.Json;

namespace NetDream.Modules.Article.Repositories
{
    public class SketchBox(ArticleContext db,
        IClientContext environment,
        byte itemType, int maxUndoCount = 1) : ISketchBox
    {
        public void Save(object data, int target = 0)
        {
            var logList = db.SketchLogs.Where(i => i.ItemType == itemType && i.ItemId == target && i.UserId == environment.UserId)
                .OrderByDescending(i => i.UpdatedAt).ToArray();
            if (logList is null || logList.Length < maxUndoCount)
            {
                db.SketchLogs.Add(new SketchLogEntity()
                {
                    ItemType = itemType,
                    ItemId = target,
                    Data = JsonSerializer.Serialize(data),
                    UserId = environment.UserId,
                    Ip = environment.Ip,
                    CreatedAt = environment.Now,
                    UpdatedAt = environment.Now,
                });
                db.SaveChanges();
                return;
            }
            var last = logList.Last();
            last.ItemType = itemType;
            last.ItemId = target;
            last.Data = JsonSerializer.Serialize(data);
            last.Ip = environment.Ip;
            last.CreatedAt = environment.Now;
            db.SketchLogs.Update(last);
            if (logList.Length > maxUndoCount)
            {
                var del = logList.Skip(maxUndoCount).Take(logList.Length - maxUndoCount);
                db.SketchLogs.RemoveRange(del);
            }
            db.SaveChanges();
        }

        /// <summary>
        /// 获取保存的全部记录
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public SketchLogEntity[] Stack(int target = 0)
        {
            return db.SketchLogs.Where(i => i.ItemType == itemType && i.ItemId == target && i.UserId == environment.UserId)
                .OrderByDescending(i => i.UpdatedAt).ToArray();
        }

        public T? Get<T>(int target = 0)
        {
            var data = db.SketchLogs.Where(i => i.ItemType == itemType && i.ItemId == target && i.UserId == environment.UserId)
                .OrderByDescending(i => i.UpdatedAt).Select(i => i.Data).Single();
            if (data is null || string.IsNullOrWhiteSpace(data))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(data);
        }

        public T? GetById<T>(int id)
        {
            var data = db.SketchLogs.Where(i => i.ItemType == itemType && i.ItemId == id && i.UserId == environment.UserId)
                .Select(i => i.Data).Single();
            if (data is null || string.IsNullOrWhiteSpace(data))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(data);
        }

        public void Remove(int target = 0)
        {
            db.SketchLogs.Where(i => i.ItemType == itemType
            && i.ItemId == target && i.UserId == environment.UserId)
                .ExecuteDelete();
            db.SaveChanges();
        }

        public void Clear()
        {
            db.SketchLogs.Where(i => i.ItemType == itemType).ExecuteDelete();
            db.SaveChanges();
        }
    }
}
