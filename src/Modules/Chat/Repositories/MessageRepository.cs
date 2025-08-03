using MediatR;
using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Chat.Entities;
using NetDream.Modules.Chat.Forms;
using NetDream.Modules.Chat.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Notifications;
using NetDream.Shared.Providers;
using NetDream.Shared.Repositories;
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
        FileRepository fileStore,
        IMediator mediator)
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

        public PingResult Ping(MessageQueryForm form) 
        {
            var res = new PingResult();
            res.MessageCount = db.Messages.Where(i => i.ReceiveId == client.UserId && i.Status == STATUS_NONE)
                .When(form.StartTime > 0, i => i.CreatedAt >= form.StartTime).Count();
            res.ApplyCount = db.Applies.Where(i => i.ItemId == client.UserId && i.Status == 0 && i.ItemType==0)
                .When(form.StartTime > 0, i => i.CreatedAt >= form.StartTime).Count();
            if (form.Id > 0) {
                var messages = db.Messages.When(form.StartTime > 0, i => i.CreatedAt >= form.StartTime)
                    .When(form.Type > 0, i => i.GroupId == form.Id, 
                    i => i.GroupId == 0 && i.ReceiveId == client.UserId && i.UserId == form.Id)
                    .SelectAs().ToArray();
                userStore.Include(messages);
                IncludeReceive(messages);
                res.Data = [new() {
                    Id = form.Id,
                    Type = form.Type,
                    Items = messages
                }];
            }
            res.NextTime = client.Now + 1;
            return res;
        }

        private void IncludeReceive(IEnumerable<MessageListItem> items)
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


        public IOperationResult<MessageEntity> SendText(int itemType, 
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
                mediator.Publish(BulletinRequest.Create([.. userIds],
                    $"我在群【{groupModel.Name}】提到了你", "[回复]", [
                        ruler.FormatLink("[回复]", "chat")
                    ], BulletinType.ChatAt));
            }
            return [..rules];
        }

        public MessageEntity[] SendImage(int itemType, 
            int id,
            IUploadFileCollection items) {
            var images = fileStore.UploadImages(items);
            var word = "[图片]";
            return SendBatch(itemType, id, client.UserId,
                images.Select(i => new MessageForm()
                {
                    Type = TYPE_IMAGE,
                    Content = word,
                    ExtraRule = [
                        ruler.FormatImage(word, i.Url)
                    ]
                }).ToArray());
        }

        public IOperationResult<MessageEntity> SendFile(int itemType,
            int id,
            IUploadFile file) {
            var res = fileStore.UploadFile(file);
            if (!res.Succeeded)
            {
                return OperationResult<MessageEntity>.Fail(res);
            }
            var word = $"[{res.Result.Original}]";
            return Send(itemType, id, client.UserId,
                    TYPE_FILE, word, [
                    ruler.FormatFile(word, res.Result.Url)
                ]);
        }

        public IOperationResult<MessageEntity> SendVideo(int itemType,
            int id,
            IUploadFile file) {
            var res = fileStore.UploadVideo(file);
            if (!res.Succeeded)
            {
                return OperationResult<MessageEntity>.Fail(res);
            }
            var word = "[视频]";
            return Send(itemType, id, client.UserId,
                    TYPE_VIDEO, word, [
                    ruler.FormatFile(word, res.Result.Url)
                ]);
        }

        public IOperationResult<MessageEntity> SendAudio(int itemType, 
            int id,
            IUploadFile file) {
            var res = fileStore.UploadAudio(file);
            if (!res.Succeeded)
            {
                return OperationResult<MessageEntity>.Fail(res);
            }
            var word = "[语音]";
            return Send(itemType, id, client.UserId,
                TYPE_VOICE, word, [
                    ruler.FormatFile(word, res.Result.Url)
                ]);
        }

        public IOperationResult<MessageEntity> Send(int itemType,
            int id,
            int user, int type,
            string content,
            LinkExtraRule[]? extraRule = null) 
        {
            if (string.IsNullOrWhiteSpace(content)) 
            {
                return OperationResult<MessageEntity>.Fail("内容不能为空");
            }
            var items = SendBatch(itemType, id, user, [
                new() {
                    Type = type,
                    Content = content,
                    ExtraRule = extraRule
                }
            ]);
            if (items.Length == 0) {
                return OperationResult<MessageEntity>.Fail("发送失败");
            }
            return OperationResult.Ok(items[0]);
        }

        /// <summary>
        /// 插入多条
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public MessageEntity[] SendBatch(int itemType, int id, 
            int user, 
            params MessageForm[] data) 
        {
            var items = new List<MessageEntity>();
            foreach (var item in data) 
            {
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
            if (items.Count == 0) 
            {
                return [];
            }
            AddHistory(itemType, id, user, items[items.Count - 1].Id, items.Count);
            return [..items];
        }


        public MessageQueryResult GetList(MessageQueryForm form) 
        {
            var res = new MessageQueryResult
            {
                NextTime = client.Now + 1
            };
            MessageListItem[] data;
            if (form.StartTime == 0) {
                data = db.Messages.When(form.Type > 0, 
                    i => i.GroupId == form.Id, 
                    i => i.GroupId == 0 && i.ReceiveId == client.UserId && i.UserId == form.Id)
                    .OrderByDescending(i => i.CreatedAt).Take(10).SelectAs().ToArray();
                data.Reverse();
            } else {
                data = db.Messages.Where(i => i.CreatedAt>= form.StartTime)
                    .When(form.Type > 0, i => i.GroupId == form.Id, 
                    i => i.GroupId == 0 && i.ReceiveId == client.UserId && i.UserId == form.Id)
                    .OrderBy(i => i.CreatedAt).SelectAs().ToArray();
            }
            userStore.Include(data);
            IncludeReceive(data);
            res.Data = data;
            var next_time = client.Now + 1;
            if (data.Length == 0 || form.Type == 0) {
                return res;
            }
            var userIds = new List<int>();
            foreach (var item in data) {
                userIds.Add(item.UserId);
                userIds.Add(item.ReceiveId);
            }
            var users = GetGroupUser(form.Id, userIds.Distinct().ToArray());
            foreach (var item in data) {
                item.User = FormatGroupUser(users, item.UserId, item.User);
                item.Receive = FormatGroupUser(users, item.ReceiveId, item.Receive);
            }
            return res;
        }

        private GroupUserModel FormatGroupUser(Dictionary<int, GroupUserEntity> groupUsers, int id, IUser user) 
        {
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

        /// <summary>
        /// 消息撤回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        public void AddHistory(int type, int id, int user_id, int message = 0, int count = 0)
        {
            AddIfHistory(type, id, user_id, message, 0);
            if (type > 0)
            {
                return;
            }
            AddIfHistory(type, user_id, id, message, count < 1 ? 1 : count);
        }

        private void AddIfHistory(int type, int id, int user_id, int message = 0, int messageCount = 0)
        {
            var count = db.Histories.Where(i => i.ItemType == type && i.ItemId == id && i.UserId == user_id).Any();
            if (count)
            {
                db.Histories.Where(i => i.ItemType == type && i.ItemId == id && i.UserId == user_id)
                    .ExecuteUpdate(setters =>
                    setters.SetProperty(i => i.UnreadCount, i => i.UnreadCount + messageCount)
                    .SetProperty(i => i.LastMessage, message));
                return;
            }
            db.Histories.Save(new HistoryEntity()
            {
                ItemType = type,
                ItemId = id,
                UserId = user_id,
                UnreadCount = messageCount,
                LastMessage = message
            }, client.Now);
        }
    }
}
