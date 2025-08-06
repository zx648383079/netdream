using Microsoft.Extensions.Caching.Memory;
using NetDream.Modules.Bot.Adapters;
using NetDream.Modules.Bot.Entities;
using NetDream.Modules.Bot.Forms;
using NetDream.Modules.Bot.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Bot.Repositories
{
    public class ReplyRepository(BotContext db, IClientContext client, IMemoryCache cache)
    {
        public const string EVENT_DEFAULT = "default";

        public Dictionary<string, string> EventItems()
        {
            return new Dictionary<string, string>()
            {
                { EVENT_DEFAULT, "默认回复" },
                {AdapterEvent.Message.ToEventName(), "消息"},
                {AdapterEvent.Subscribe.ToEventName(), "关注"},
                {AdapterEvent.MenuClick.ToEventName(), "菜单事件"},
            };
        }

        public IPage<ReplyEntity> GetList(int bot_id, ReplyQueryForm form) 
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return new Page<ReplyEntity>();
            }
            return db.Replies.Where(i => i.BotId == bot_id)
                .When(form.Event, i => i.Event == form.Event)
                .OrderByDescending(i => i.Status)
                .ThenByDescending(i => i.Id).ToPage(form);
        }

        public IPage<ReplyEntity> ManageList(int bot_id, ReplyQueryForm form)
        {
            return db.Replies.When(bot_id > 0, i => i.BotId == bot_id)
                .When(form.Event, i => i.Event == form.Event)
                .OrderByDescending(i => i.Status)
                .ThenByDescending(i => i.Id).ToPage(form);
        }

        public IOperationResult<ReplyEntity> Get(int id)
        {
            var model = db.Replies.Where(i => i.Id == id).SingleOrDefault();
            return OperationResult.OkOrFail(model, "数据有误");
        }

        public IOperationResult<ReplyEntity> SelfGet(int id)
        {
            var model = db.Replies.Where(i => i.Id == id).SingleOrDefault();
            if (model == null || !AccountRepository.IsSelf(db, client, model.BotId))
            {
                return OperationResult.Fail<ReplyEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult Remove(int id)
        {
            var model = db.Replies.Where(i => i.Id == id).SingleOrDefault();
            if (model == null || !AccountRepository.IsSelf(db, client, model.BotId))
            {
                return OperationResult.Fail<ReplyEntity>("数据有误");
            }
            db.Replies.Remove(model);
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult<ReplyEntity> GetByEvent(int bot_id, string eventName)
        {
            var model = db.Replies.Where(i => i.BotId == bot_id && i.Event == eventName)
                .FirstOrDefault();
            return OperationResult.OkOrFail(model, "数据有误");
        }

        public IOperationResult<ReplyEntity> Save(int bot_id, ReplyForm data)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return OperationResult.Fail<ReplyEntity>("权限不足");
            }
            var model = data.Id > 0 ? db.Replies.Where(i => i.Id == data.Id && i.BotId == bot_id)
                .SingleOrDefault() : new ReplyEntity();
            if (model is null)
            {
                return OperationResult.Fail<ReplyEntity>("数据错误");
            }
            model.Event = data.Event;
            model.BotId = bot_id;
            model.Content = data.Content;
            model.Keywords = data.Keywords;
            model.Match = data.Match;
            if (model.Event != AdapterEvent.Message.ToEventName())
            {
                model.Keywords = string.Empty;
            }
            // EditorInput.Save(model, input);
            db.Replies.Save(model, client.Now);
            db.SaveChanges();
            CacheReply(model.BotId, true);
            return OperationResult.Ok(model);
        }

        public IOperationResult<ReplyEntity> Update(int id, string[] data)
        {
            var model = db.Replies.Where(i => i.Id == id).SingleOrDefault();
            if (model == null || !AccountRepository.IsSelf(db, client, model.BotId))
            {
                return OperationResult.Fail<ReplyEntity>("数据有误");
            }
            if (!data.Contains("status"))
            {
                return OperationResult.Ok(model);
            }
            model.Status = (byte)(model.Status > 0 ? 0 : 1);
            db.Replies.Update(model);
            db.SaveChanges();
            CacheReply(model.BotId, true);
            return OperationResult.Ok(model);
        }

        public IOperationResult Send(int bot_id, int toType, int to, Dictionary<string, string> data)
        {
            if (data["type"] == 3)
            {
                return SendTemplate(bot_id, to, data);
            }
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return OperationResult.Fail("权限不足");
            }
            var content = string.Empty;
            if (data["type"] < 1)
            {
                content = data["text"];
            }
            var adapter = new BotRepository().Entry(bot_id);
            if (toType < 1)
            {
                return adapter.SendUsers(content);
            }
            if (toType == 1)
            {
                var groupId = db.UserGroups.Where(i => i.Id == to && i.TagId != string.Empty)
                    .Value(i => i.TagId);
                return adapter.SendGroup(groupId, content);
            }
            var openId = db.Users.Where(i => i.Id == to).Pluck(i => i.Openid);
            return adapter.SendAnyUsers(openId, content);
        }

        public IOperationResult SendTemplate(int bot_id, int userId, Dictionary<string, string> data)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return OperationResult.Fail("权限不足");
            }
            if (userId < 1)
            {
                return OperationResult.Fail("模板消息只能发给单个用户");
            }
            var openid = db.Users.Where(i => i.Id == userId).Value(i => i.Openid);
            if (string.IsNullOrEmpty(openid))
            {
                return OperationResult.Fail("用户未关注公众号");
            }
            data["template_data"] = ToArray(data["template_data"]);
            return new BotRepository().Entry(bot_id)
                .SendTemplate(openid, data);
        }

        public static Dictionary<string, string> ToArray(string text)
        {
            var res = new Dictionary<string, string>();
            foreach (var item in text.Split(['\n', '\r']))
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                var args = item.Split('=', 2);
                if (string.IsNullOrWhiteSpace(args[0]))
                {
                    continue;
                }
                if (args.Length > 1)
                {
                    res.Add(args[0], args[1]);
                    continue;
                }
                res.Add(args[0], string.Empty);
                continue;
            }
            return res;
        }

        public IPage<TemplateEntity> TemplateList(int bot_id, QueryForm form)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return new Page<TemplateEntity>();
            }
            return db.Templates.Where(i => i.BotId == bot_id).ToPage(form);
        }

        public IOperationResult SyncTemplate(int bot_id)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return OperationResult.Fail("权限不足");
            }
            new BotRepository().Entry(bot_id)
                .PullTemplate(item => {
                    var templateId = item["template_id"];
                    var model = db.Templates.Where(i => i.BotId == bot_id && i.TemplateId == templateId)
                        .SingleOrDefault();
                    if (model is null)
                    {
                        model = new TemplateEntity();
                    }
                    model.BotId = bot_id;
                    model.TemplateId = templateId;
                    model.Title = item["title"];
                    model.Content = item["content"];
                    model.Example = item["example"]
                    db.Templates.Save(model);
            });
            db.SaveChanges();
        }

        public string Preview(TemplateEntity entity, Dictionary<string, string> data)
        {
            return Regex.Replace(entity.Content, @"\{\s?\{([^\{]+)\.DATA\}\s?\}", match => {
                var key = match.Groups[1].Value.Trim();
                return data.TryGetValue(key, out var val) ? val : string.Empty;
            });
        }

        public string[] GetField(TemplateEntity entity)
        {
            return Regex.Matches(entity.Content, @"\{([^\{]+)\.DATA\}").Select(i => i.Groups[1].Value).ToArray();
        }

        public string[] Template(int id)
        {
            var model = TemplateDetail(id);
            if (!model.Succeeded)
            {
                return [];
            }
            return GetField(model.Result);
        }

        public IOperationResult<TemplateEntity> TemplateDetail(int id)
        {
            var model = db.Templates.Where(i => i.Id == id).SingleOrDefault();
            if (model is null || !AccountRepository.IsSelf(db, client, model.BotId))
            {
                return OperationResult.Fail<TemplateEntity>("模板不存在");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<string> TemplatePreview(int id, Dictionary<string, string> data)
        {
            var model = TemplateDetail(id);
            if (!model.Succeeded)
            {
                return OperationResult<string>.Fail(model);
            }
            return OperationResult.Ok(Preview(model.Result, data));
        }

        public OptionItem<string>[] SceneList()
        {
            return [];
        }

        public IOperationResult<TemplateEntity> TemplateSave(int bot_id, TemplateForm data)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return OperationResult.Fail<TemplateEntity>("模板不存在");
            }
            var model = data.Id > 0 ? 
                db.Templates.Where(i => i.Id == data.Id && i.BotId == bot_id)
                .SingleOrDefault() : new TemplateEntity();
            if (model is null)
            {
                return OperationResult.Fail<TemplateEntity>("模板不存在");
            }
            model.BotId = bot_id;
            model.Title = data.Title;
            model.Content = data.Content;
            model.Example = data.Example;
            db.Templates.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult TemplateRemove(int id)
        {
            var model = db.Templates.Where(i => i.Id == id).SingleOrDefault();
            if (model is null || !AccountRepository.IsSelf(db, client, model.BotId))
            {
                return OperationResult.Fail("模板不存在");
            }
            db.Templates.Remove(model);
            db.SaveChanges();
            return OperationResult.Ok();
        }


        public ReplyMatchCollection GetMessageReply(int bot_id)
        {
            var eventName = AdapterEvent.Message.ToEventName();
            var data = db.Replies.Where(i => i.Event == eventName 
                && i.BotId == bot_id && i.Status == 1)
                .OrderByDescending(i => i.Match)
                .ThenBy(i => i.UpdatedAt)
                .Select(i => new ReplyEntity()
                {
                    Id = i.Id,
                    Keywords = i.Keywords,
                    Match = i.Match,
                }).ToArray();
            var res = new ReplyMatchCollection
            {
                data
            };
            return res;
        }

        public ReplyMatchCollection CacheReply(int bot_id, bool refresh = false)
        {
            var key = "wx_reply_" + bot_id;
            ReplyMatchCollection res;
            if (refresh)
            {
                res = GetMessageReply(bot_id);
                cache.Set(key, res);
            } else
            {
                res = cache.GetOrCreate(key, _ => {
                    return GetMessageReply(bot_id);
                });
            }
            return res;
        }

        public int FindIdWithCache(int bot_id, string content)
        {
            var data = CacheReply(bot_id);
            if (data.TryGet(content, out var id))
            {
                return id;
            }
            return 0;
        }

        public ReplyEntity? FindWithCache(int bot_id, string content)
        {
            var id = FindIdWithCache(bot_id, content);
            return id > 0 ? db.Replies.Where(i => i.BotId == bot_id && i.Id == id)
                .SingleOrDefault() : null;
        }

        public ReplyEntity? FindWithEvent(string eventName, int bot_id)
        {
            return db.Replies.Where(i => i.Event == eventName && i.BotId == bot_id && i.Status == 1)
                .FirstOrDefault();
        }
    }
}
