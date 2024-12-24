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
        public IList<MessageModel> GetList(int sessionId, 
            int startTime, int lastId = 0)
        {
            var query = db.Messages.Where(i => i.SessionId == sessionId);
            if (startTime <= 0)
            {
                query = query.When(lastId > 0, i => i.Id < lastId)
                    .OrderByDescending(i => i.CreatedAt)
                    .Take(10);
            }
            else
            {
                query = query.Where(i => i.CreatedAt >= startTime)
                    .OrderBy(i => i.CreatedAt);
            }
            var data = query.ToArray().CopyTo<MessageEntity, MessageModel>();
            userStore.WithUser(data);
            var guest = new GuestUser();
            foreach (var item in data)
            {
                item.User??= guest;
            }
            if (startTime <= 0)
            {
                data.Reverse();
            }
            return data;
        }

        public void Send(int sessionId, MessageForm data, byte sendType = 0)
        {
            var session = db.Sessions.Where(i => i.Id == sessionId).Single();
            if (session is null)
            {
                return;
            }
            if (sendType < 1)
            {
                session.UpdatedAt = client.Now;
                db.Sessions.Update(session);
                db.SaveChanges();
            }
            if (!SendMessage(sessionId, data, sendType)) {
                return;
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
        }

        public bool SendMessage(int sessionId, MessageForm data, byte sendType = 0)
        {
            if (data.Content is string)
            {
                return CreateMessage(sessionId, data.Content,
                    data.Type, sendType);
            }
            var success = false;
            //foreach (var item in data.Content)
            //{
            //    if (string.IsNullOrWhiteSpace(item))
            //    {
            //        continue;
            //    }
            //    if (CreateMessage(sessionId, item, data.Type, sendType)) {
            //        success = true;
            //    }
            //}
            return success;
        }

        public bool CreateMessage(int sessionId, string content, 
            byte type = 0, byte sendType = 0, int userId = -1)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return false;
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
            return true;
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
    }
}
