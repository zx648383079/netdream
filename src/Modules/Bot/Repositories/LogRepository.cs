using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Bot.Forms;
using NetDream.Modules.Bot.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Bot.Repositories
{
    public class LogRepository(BotContext db, IClientContext client)
    {
        public IPage<HistoryListItem> GetList(int bot_id, LogQueryForm form)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return new Page<HistoryListItem>();
            }
            return ManageList(bot_id, form);
        }

        public IPage<HistoryListItem> ManageList(int bot_id, LogQueryForm form)
        {
            var items = db.MessageHistories
                .When(bot_id > 0, i => i.BotId == bot_id)
                .When(form.Mark > 0, i => i.IsMark == 1)
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            if (items.Items.Length == 0)
            {
                return items;
            }
            var idItems = new HashSet<string>();
            foreach (var item in items.Items)
            {
                idItems.Add(item.From);
                idItems.Add(item.To);
            }
            var data = db.Users.Where(i => idItems.Contains(i.Openid))
                .Select(i => new UserLabelItem()
                {
                    Id = i.Id,
                    Nickname = i.Nickname,
                    NoteName = i.NoteName,
                    Avatar = i.Avatar,
                    Openid = i.Openid,
                }).ToDictionary(i => i.Openid);
            foreach (var item in items.Items)
            {
                if (data.TryGetValue(item.From, out var res))
                {
                    item.FromUser = res;
                }
                if (data.TryGetValue(item.To, out res))
                {
                    item.ToUser = res;
                }
            }
            return items;
        }
        public IOperationResult Mark(int id)
        {
            var log = db.MessageHistories.Where(i => i.Id == id).SingleOrDefault();
            if (log is null || !AccountRepository.IsSelf(db, client, log.BotId))
            {
                return OperationResult.Fail("记录不存在");
            }
            db.MessageHistories.Where(i => i.Id == id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.IsMark, log.IsMark > 0 ? 0 : 1));
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult Remove(int id)
        {
            var log = db.MessageHistories.Where(i => i.Id == id).SingleOrDefault();
            if (log is null || !AccountRepository.IsSelf(db, client, log.BotId))
            {
                return OperationResult.Fail("记录不存在");
            }
            db.MessageHistories.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }
    }
}
