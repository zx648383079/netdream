using NetDream.Modules.OnlineService.Entities;
using NetDream.Modules.OnlineService.Forms;
using NetDream.Modules.OnlineService.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NetDream.Modules.OnlineService.Repositories
{
    public class ChatRepository(OnlineServiceContext db, 
        IUserRepository userStore,
        IClientContext client,
        ILinkRuler ruler)
    {
        public const byte TYPE_TEXT = 0;
        public const byte TYPE_EMOJI = 1;
        public const byte TYPE_IMAGE = 2;
        public MessageListItem[] GetList(MessageQueryForm form)
        {
            var query = db.Messages.Where(i => i.SessionId == form.SessionId);
            if (form.StartTime <= 0)
            {
                query = query.When(form.LastId > 0, i => i.Id < form.LastId)
                    .OrderByDescending(i => i.CreatedAt)
                    .Take(10);
            }
            else
            {
                query = query.Where(i => i.CreatedAt >= form.StartTime)
                    .OrderBy(i => i.CreatedAt);
            }
            var data = query.SelectAs().ToArray();
            userStore.Include(data);
            var guest = new GuestUser();
            foreach (var item in data)
            {
                item.User??= guest;
            }
            if (form.StartTime <= 0)
            {
                return data.Reverse().ToArray();
            }
            return data;
        }

        public IOperationResult Send(int sessionId, MessageForm data, byte sendType = 0)
        {
            var session = db.Sessions.Where(i => i.Id == sessionId).SingleOrDefault();
            if (session is null)
            {
                return OperationResult.Fail("session 错误");
            }
            if (sendType < 1)
            {
                session.UpdatedAt = client.Now;
                db.Sessions.Update(session);
                db.SaveChanges();
            }
            var res = SendMessage(sessionId, data, sendType);
            if (!res.Succeeded) {
                return res;
            }
            if (sendType < 1 && session.ServiceWord > 0)
            {
                CreateMessage(sessionId,
                    db.CategoryWords.Where(i => i.Id == session.ServiceWord).Select(i => i.Content).Single()
                    , 0, 1, session.ServiceId);
            }
            var service_id = client.UserId;
            if (sendType > 0 && session.ServiceId != service_id)
            {
                session.ServiceId = service_id;
                session.Status = 1;
                db.Sessions.Save(session);
                db.SessionLogs.Save(new SessionLogEntity() {
                    UserId = service_id,
                    SessionId = sessionId,
                    Status = session.Status,
                    Remark = string.Format("客服 【{0}】 开始接待", client.UserId)
                }, client.Now);
                db.SaveChanges();
            }
            return OperationResult.Ok();
        }

        public IOperationResult SendMessage(int sessionId, MessageForm data, byte sendType = 0)
        {
            return CreateMessage(sessionId, data.Content,
                    data.Type, sendType);
        }

        public IOperationResult SendMessage(int sessionId, MessageForm[] data, byte sendType = 0)
        {
            foreach (var item in data)
            {
                if (string.IsNullOrWhiteSpace(item.Content))
                {
                    continue;
                }
                var res = CreateMessage(sessionId, item.Content, item.Type, sendType);
                if (!res.Succeeded)
                {
                    return res;
                }
            }
            return OperationResult.Ok();
        }

        public IOperationResult CreateMessage(int sessionId, string content, 
            byte type = 0, byte sendType = 0, int userId = -1)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return OperationResult.Fail("内容不能为空");
            }
            var extra_rule = new List<LinkExtraRule>();
            if (type == 1)
            {
                extra_rule.Add(ruler.FormatImage("[图片]", content));
                content = "[图片]";
            }
            else
            {
                extra_rule.AddRange(ruler.FormatEmoji(content));
            }
            db.Messages.Add(new MessageEntity()
            {
                UserId = client.UserId,
                SessionId =sessionId,
                SendType = sendType,
                Type = type,
                Content = content,
                ExtraRule = JsonSerializer.Serialize(extra_rule),
                CreatedAt = client.Now,
                UpdatedAt = client.Now,
            });
            db.SaveChanges();
            return OperationResult.Ok(true);
        }

        public int SessionId(string session_token = "")
        {
            if (int.TryParse(session_token, out var id))
            {
                return id;
            }
            if (client.UserId > 0)
            {
                id = db.Sessions.Where(i => i.UserId == client.UserId).Max(i => i.Id);
                if (id > 0)
                {
                    return id;
                }
            }
            var model = new SessionEntity()
            {
                Name = client.UserId == 0 ? "游客_" + client.Now :
                client.UserId.ToString(),
                UserId = client.UserId,
                Ip = client.Ip,
                UserAgent = client.UserAgent,
                CreatedAt = client.Now,
                UpdatedAt = client.Now,
            };
            db.Sessions.Add(model);
            db.SaveChanges();
            return model.Id;
        }

        public string EncodeSession(int sessionId)
        {
            return sessionId.ToString();
        }

        public MessageQueryResult MessageList(MessageQueryForm form)
        {
            form.SessionId = SessionId(form.SessionToken);
            var res = new MessageQueryResult
            {
                Data = GetList(form),
                SessionToken = EncodeSession(form.SessionId),
                NextTime = client.Now + 1
            };
            return res;
        }

        public bool CSEnabled(int sessionId)
        {
            return new SessionRepository(db, client, userStore).HasRole(sessionId);
        }

        public MessageQueryResult CSMessageList(MessageQueryForm form)
        {
            var res = new MessageQueryResult
            {
                Data = GetList(form),
                NextTime = client.Now + 1
            };
            return res;
        }
    }
}
