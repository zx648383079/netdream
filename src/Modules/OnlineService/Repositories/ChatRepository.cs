using Modules.OnlineService.Entities;
using NetDream.Modules.OnlineService.Forms;
using NetDream.Modules.OnlineService.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Shared.Models;
using NPoco;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetDream.Modules.OnlineService.Repositories
{
    public class ChatRepository(IDatabase db, 
        IUserRepository userStore,
        IClientContext environment,
        ILinkRuler ruler)
    {
        public IList<MessageModel> GetList(int sessionId, 
            int startTime, int lastId = 0)
        {
            var sql = new Sql();
            sql.Select("*").From<MessageEntity>(db)
                .Where("session_id=@0", sessionId);
            if (startTime <= 0)
            {
                if (lastId > 0)
                {
                    sql.Where("id<@0", lastId);
                }
                sql.OrderBy("created_at DESC")
                    .Limit(10);
            }
            else
            {
                sql.Where("created_at>=@0", startTime)
                    .OrderBy("created_at ASC");
            }
            var data = db.Fetch<MessageModel>(sql);
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
            var session = db.SingleById<SessionEntity>(sessionId);
            if (session is null)
            {
                return;
            }
            if (sendType < 1)
            {
                db.Update<SessionEntity>("SET updated_at=@0 WHERE id=@1", environment.Now,
                    sessionId);
            }
            if (!SendMessage(sessionId, data, sendType)) {
                return;
            }
            if (sendType < 1 && session.ServiceWord > 0)
            {
                CreateMessage(sessionId,
                    db.FindScalar<string, CategoryWordEntity>("content", "id=@0", session.ServiceWord)
                    , 0, 1, session.ServiceId);
            }
            var service_id = environment.UserId;
            if (sendType > 0 && session.ServiceId != service_id)
            {
                session.ServiceId = service_id;
                session.Status = 1;
                db.TrySave(session);
                db.Insert(new SessionLogEntity() {
                    UserId = service_id,
                    SessionId = sessionId,
                    Status = session.Status,
                    Remark = string.Format("客服 【{0}】 开始接待", environment.UserId)
                });
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
            db.Insert(new MessageEntity()
            {
                UserId = environment.UserId,
                SessionId =sessionId,
                SendType = sendType,
                Type = type,
                Content = content,
                ExtraRule = JsonSerializer.Serialize(extra_rule),
                CreatedAt = environment.Now,
                UpdatedAt = environment.Now,
            });
            return true;
        }

        public int SessionId(string session_token = "")
        {
            if (int.TryParse(session_token, out var id))
            {
                return id;
            }
            if (environment.UserId > 0)
            {
                id = db.FindScalar<int, SessionEntity>("MAX(id)",
                    "user_id=@0", environment.UserId);
                if (id > 0)
                {
                    return id;
                }
            }
            id = (int)db.Insert(new SessionEntity()
            {
                Name = environment.UserId == 0 ? "游客_" + environment.Now :
                environment.UserId.ToString(),
                UserId = environment.UserId,
                Ip = environment.Ip,
                UserAgent = environment.UserAgent,
                CreatedAt = environment.Now,
                UpdatedAt = environment.Now,
            });
            return id;
        }

        public string EncodeSession(int sessionId)
        {
            return sessionId.ToString();
        }
    }
}
