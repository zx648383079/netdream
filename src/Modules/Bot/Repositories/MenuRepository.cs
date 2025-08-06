using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Bot.Entities;
using NetDream.Modules.Bot.Forms;
using NetDream.Modules.Bot.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Bot.Repositories
{
    public class MenuRepository(BotContext db, IClientContext client)
    {
        public ITreeItem[] GetList(int bot_id)
        {
            var items = db.Menus.Where(i => i.BotId == bot_id)
                .OrderBy(i => i.ParentId)
                .Select(i => new MenuTreeItem()
                {
                    Id = i.Id,
                    BotId = i.BotId,
                    Name = i.Name,
                    Content = i.Content,
                    ParentId = i.ParentId,
                    Type = i.Type,
                })
                .ToArray();
            return TreeHelper.Create(items);
        }

        public ITreeItem[] SelfList(int bot_id)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return [];
            }
            return GetList(bot_id);
        }

        public IPage<MenuEntity> ManageList(int bot_id, QueryForm form)
        {
            return db.Menus.When(bot_id > 0, i => i.BotId == bot_id)
                .ToPage(form);
        }

        public IOperationResult<MenuEntity> Get(int id)
        {
            var model = db.Menus.Where(i => i.Id == id).SingleOrDefault();
            return OperationResult.OkOrFail(model, "菜单项错误");
        }

        public IOperationResult<MenuEntity> GetSelf(int id)
        {
            var model = db.Menus.Where(i => i.Id == id).SingleOrDefault();
            if (model is null || !AccountRepository.IsSelf(db, client, model.BotId))
            {
                return OperationResult.Fail<MenuEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult Remove(int id)
        {
            var model = db.Menus.Where(i => i.Id == id).SingleOrDefault();
            if (model is null || !AccountRepository.IsSelf(db, client, model.BotId))
            {
                return OperationResult.Fail<MenuEntity>("数据有误");
            }
            db.Menus.Remove(model);
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult<MenuEntity> Save(int bot_id, MenuForm data)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return OperationResult.Fail<MenuEntity>("权限不足");
            }
            var model = data.Id > 0 ? db.Menus.Where(i => i.Id == data.Id && i.BotId == bot_id)
                .SingleOrDefault() : new MenuEntity();
            if (model is null)
            {
                return OperationResult.Fail<MenuEntity>("数据错误");
            }
            model.Name = data.Name;
            model.BotId = bot_id;
            model.Content = data.Content;
            model.ParentId = data.ParentId;
            // EditorInput.Save(model, input);
            db.Menus.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult BatchSave(int bot_id, MenuForm[] items)
        {
            var exist = new HashSet<int>();
            foreach (var item in items)
            {
                var model = Save(bot_id, item);
                if (!model.Succeeded)
                {
                    continue;
                }
                exist.Add(model.Result.Id);
                if (item.Children?.Length > 0)
                {
                    foreach (var child in item.Children)
                    {
                        child.ParentId = model.Result.Id;
                        var m = Save(bot_id, child);
                        if (m.Succeeded)
                        {
                            exist.Add(m.Result.Id);
                        }
                    }
                }
            }
            db.Menus.Where(i => i.BotId == bot_id && !exist.Contains(i.Id)).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult Sync(int bot_id)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return OperationResult.Fail("数据错误");
            }
            return new BotRepository().Entry(bot_id).PushMenu(
                GetList(bot_id)
            );
        }
    }
}
