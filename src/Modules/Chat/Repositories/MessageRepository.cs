using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Chat.Entities;
using NetDream.Modules.Chat.Forms;
using NetDream.Modules.Chat.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Chat.Repositories
{
    public class MessageRepository(ChatContext db, 
        IClientContext client,
        IUserRepository userStore,
        ILinkRuler ruler,
        ISystemBulletin bulletin,
        ChatRepository chatStore)
    {
        public const int TYPE_TEXT = 0;
        public const int TYPE_IMAGE = 1;
        public const int TYPE_VIDEO = 2;
        public const int TYPE_VOICE = 3;
        public const int TYPE_FILE = 4;
        public const int TYPE_BONUS = 5; //红包

        public const int STATUS_NONE = 0;
        public const int STATUS_READ = 1;  //已读
        public const int STATUS_RECEIVED = 2; // 接受

        public object Ping(int time = 0, int type = 0, int id = 0) {
            var message = db.Messages.Where(i => i.ReceiveId == client.UserId && i.Status == STATUS_NONE)
                .When(time > 0, i => i.CreatedAt >= time).Count();
            var apply = db.Applies.Where(i => i.ItemId == client.UserId && i.Status == 0 && i.ItemType==0)
                .When(time > 0, i => i.CreatedAt >= time).Count();
            MessageModel[] messages = [];
            if (id > 0) {
                messages = db.Messages.When(time > 0, i => i.CreatedAt >= time)
                    .When(type > 0, i => i.GroupId == id, i => i.GroupId == 0 && i.ReceiveId == client.UserId && i.UserId == id)
                    .ToArray().CopyTo<MessageEntity, MessageModel>();
                userStore.WithUser(messages);
                WithReceive(messages);
            }
            time = client.Now + 1;
            return new Dictionary<string, object>()
            {
                {"message_count", message },
                {"apply_count", apply },
                {"next_time", time },
                {"data", new object[]{
                        new {
                            type,
                            id,
                            items = messages
                        }
                    }
                }
            };
        }

        private void WithReceive(IEnumerable<MessageModel> items)
        {
            var idItems = items.Select(item => item.ReceiveId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = userStore.Get(idItems.ToArray());
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.ReceiveId == it.Id)
                    {
                        item.Receive = it;
                        break;
                    }
                }
            }
        }


        public MessageEntity SendText(int itemType, 
            int id, 
            string content) {
            var extraRules = new List<LinkExtraRule>();
            if (itemType > 0)
            {
                // 只有群才能at 群人名
                extraRules.AddRange(At(content, id));
            }
            extraRules.AddRange(ruler.FormatEmoji(content));
            return Send(itemType, id, client.UserId,
                TYPE_TEXT, content, [..extraRules]);
        }

        public LinkExtraRule[] At(string content, int group) {
            if (string.IsNullOrWhiteSpace(content) || !content.Contains('@')) {
                return [];
            }
            var matches = Regex.Matches(content, @"@(\S+?)\s");
            if (matches is null || matches.Count == 0)
            {
                return [];
            }
            var names = matches.ToDictionary(i => i.Groups[1].Value, i => i.Value);
      
            var users = db.GroupUsers.Where(i => names.Keys.Contains(i.Name))
                .Select(i => new GroupUserEntity()
                {
                    UserId = i.UserId,
                    Name = i.Name,
                }).ToArray();
            if (users.Length == 0) {
                return [];
            }
            var rules = new List<LinkExtraRule>();
            var currentUser = client.UserId;
            var userIds = new List<int>();
            foreach (var user in users) {
                if (user.UserId != currentUser) {
                    userIds.Add(user.UserId);
                }
                rules.Add(ruler.FormatUser(names[user.Name], user.Id));
            }
            if (userIds.Count > 0) {
                var groupModel = db.Groups.Where(i => i.Id == group).Single();
                bulletin.Message([.. userIds],
                    $"我在群【{groupModel.Name}】提到了你", "[回复]", 88, [
                        ruler.FormatLink("[回复]", "chat")
                    ]);
            }
            return [..rules];
        }

        public MessageEntity[] SendImage(int itemType, 
            int id,
            string[] imageItems) {
            // images = FileRepository.UploadImages(fieldKey);
            var word = "[图片]";
            return SendBatch(itemType, id, client.UserId, 
                imageItems.Select(i => new MessageForm()
                {
                    Type = TYPE_IMAGE,
                    Content = word,
                    ExtraRule = [
                        ruler.FormatImage(word, i)
                    ]
                }).ToArray());
        }

        public MessageEntity SendFile(int itemType,
            int id,
            string fileName,
            string url) {
            // file = FileRepository.UploadFile(fieldKey);
            var word = $"[{fileName}]";
            return Send(itemType, id, client.UserId,
                    TYPE_FILE, word, [
                    ruler.FormatFile(word, url)
                ]);
        }

        public MessageEntity SendVideo(int itemType,
            int id,
            string url) {
            // file = FileRepository.UploadVideo(fieldKey);
            var word = "[视频]";
            return Send(itemType, id, client.UserId,
                    TYPE_VIDEO, word, [
                    ruler.FormatFile(word, url)
                ]);
        }

        public MessageEntity SendAudio(int itemType, 
            int id,
            string url) {
            // file = FileRepository.UploadVideo(fieldKey);
            var word = "[语音]";
            return Send(itemType, id, client.UserId,
                TYPE_VOICE, word, [
                    ruler.FormatFile(word, url)
                ]);
        }

        public MessageEntity Send(int itemType,
            int id,
            int user, int type,
            string content,
            LinkExtraRule[]? extraRule = null) {
            if (string.IsNullOrWhiteSpace(content)) {
                throw new Exception("内容不能为空");
            }
            var items = SendBatch(itemType, id, user, [
                new() {
                    Type = type,
                    Content = content,
                    ExtraRule = extraRule
                }
            ]);
            if (items.Length == 0) {
                throw new Exception("发送失败");
            }
            return items[0];
        }

        /**
         * 插入多条
         * @param int itemType
         * @param int id
         * @param int user
         * @param array data  [[type, content, extra_rule]]
         * @return array
         */
        public MessageEntity[] SendBatch(int itemType, int id, 
            int user, 
            params MessageForm[] data) {
            var items = new List<MessageEntity>();
            foreach (var item in data) {
                var message = new MessageEntity()
                {
                    Type = item.Type,
                    Content = item.Content,
                    ItemId = item.ItemId,
                    ReceiveId = itemType < 1 ? id : 0,
                    GroupId = itemType < 1 ? 0 : id,
                    UserId = user,
                    Status = STATUS_NONE,
                    DeletedAt = 0,
                    ExtraRule = JsonSerializer.Serialize(item.ExtraRule) ?? string.Empty,
                };
                db.Messages.Save(message, client.Now);
                if (db.SaveChanges() > 0) {
                    items.Add(message);
                }
            }
            if (items.Count == 0) {
                throw new Exception("发送失败");
            }
            chatStore.AddHistory(itemType, id, user, items[items.Count - 1].Id, items.Count);
            return [..items];
        }


        public object GetList(int itemType, int id, int startTime) {
            MessageModel[] data;
            if (startTime == 0) {
                data = db.Messages.When(itemType > 0, i => i.GroupId == id, i => i.GroupId == 0 && i.ReceiveId == client.UserId && i.UserId == id)
                    .OrderByDescending(i => i.CreatedAt).Take(10).ToArray().CopyTo<MessageEntity, MessageModel>();
                data.Reverse();
            } else {
                data = db.Messages.Where(i => i.CreatedAt>= startTime)
                    .When(itemType > 0, i => i.GroupId == id, i => i.GroupId == 0 && i.ReceiveId == client.UserId && i.UserId == id)
                    .OrderBy(i => i.CreatedAt).ToArray().CopyTo<MessageEntity, MessageModel>(); ;
            }
            userStore.WithUser(data);
            WithReceive(data);
            var next_time = client.Now + 1;
            if (data.Length == 0 || itemType == 0) {
                return new {
                    next_time,
                    data
                };
            }
            var userIds = new List<int>();
            foreach (var item in data) {
                userIds.Add(item.UserId);
                userIds.Add(item.ReceiveId);
            }
            var users = GetGroupUser(id, userIds.Distinct().ToArray());
            foreach (var item in data) {
                item.User = FormatGroupUser(users, item.UserId, item.User);
                item.Receive = FormatGroupUser(users, item.ReceiveId, item.Receive);
            }
            return new {
                next_time,
                data
            };
        }

        protected GroupUserModel FormatGroupUser(Dictionary<int, GroupUserEntity> groupUsers, int id, IUser user) {
            if (groupUsers.TryGetValue(id, out var u)) {
                return new GroupUserModel(u) 
                {
                    User = user
                };
            }
            return new GroupUserModel()
            {
                Name = "[]",
                UserId = user.Id,
                User = user,
            };
        }

        protected Dictionary<int, GroupUserEntity> GetGroupUser(
            int id, params int[] ids) {
            if (ids.Length == 0) {
                return [];
            }
            return db.GroupUsers.Where(i => i.GroupId == id && ids.Contains(i.UserId)).ToDictionary(i => i.UserId);
        }

        /**
         * 消息撤回
         * @param int id
         * @throws \Exception
         */
        public IOperationResult Revoke(int id) 
        {
            var model = db.Messages.Where(i => i.Id == id).Single();
            if (model is null) {
                return OperationResult.Fail("消息错误");
            }
            if (model.UserId != client.UserId) {
                return OperationResult.Fail("操作错误");
            }
            if (model.CreatedAt < client.Now - 120) {
                return OperationResult.Fail("超过两分钟无法撤回");
            }
            db.Messages.Where(i => i.Id == id).ExecuteDelete();
            return OperationResult.Ok();
        }
        
    }
}
