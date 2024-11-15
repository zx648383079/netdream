using Google.Protobuf;
using NetDream.Modules.Chat.Entities;
using NetDream.Modules.Chat.Forms;
using NetDream.Modules.Chat.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Migrations;
using NetDream.Shared.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Chat.Repositories
{
    public class MessageRepository(IDatabase db, 
        IClientEnvironment environment,
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
            var sql = new Sql();
            sql.Select("COUNT(*) AS count")
                .From<MessageEntity>(db)
                .Where("receive_id=@0 and status=@1", 
                environment.UserId, STATUS_NONE);
            if (time > 0)
            {
                sql.Where("created_at>=@0", time);
            }
            var message = db.ExecuteScalar<int>(sql);
            sql = new Sql();
            sql.Select("COUNT(*) AS count")
                .From<ApplyEntity>(db)
                .Where("item_id=@0 and status=0 and item_type=0",
                environment.UserId);
            if (time > 0)
            {
                sql.Where("created_at>@0", time);
            }
            var apply = db.ExecuteScalar<int>(sql);
            MessageModel[] messages = [];
            if (id > 0) {
                sql = new Sql();
                sql.Select()
                    .From<MessageEntity>(db);
                if (time > 0)
                {
                    sql.Where("created_at>=@0", time);
                }
                if (type > 0)
                {
                    sql.Where("group_id=@0", id);
                } else
                {
                    sql.Where("group_id=0 and receive_id=@0 and user_id=@1", 
                        environment.UserId, id);
                }
                messages = db.Fetch<MessageModel>(sql).ToArray();
                userStore.WithUser(messages);
                WithReceive(messages);
            }
            time = environment.Now + 1;
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
            return Send(itemType, id, environment.UserId,
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
            var sql = new Sql();
            sql.Select("user_id", "name").From<GroupUserEntity>(db)
                .WhereIn("name", names.Keys.ToArray());
            var users = db.Fetch<GroupUserEntity>(sql);
            if (users.Count == 0) {
                return [];
            }
            var rules = new List<LinkExtraRule>();
            var currentUser = environment.UserId;
            var userIds = new List<int>();
            foreach (var user in users) {
                if (user.UserId != currentUser) {
                    userIds.Add(user.UserId);
                }
                rules.Add(ruler.FormatUser(names[user.Name], user.Id));
            }
            if (userIds.Count > 0) {
                var groupModel = db.SingleById<GroupEntity>(group);
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
            return SendBatch(itemType, id, environment.UserId, 
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
            return Send(itemType, id, environment.UserId,
                    TYPE_FILE, word, [
                    ruler.FormatFile(word, url)
                ]);
        }

        public MessageEntity SendVideo(int itemType,
            int id,
            string url) {
            // file = FileRepository.UploadVideo(fieldKey);
            var word = "[视频]";
            return Send(itemType, id, environment.UserId,
                    TYPE_VIDEO, word, [
                    ruler.FormatFile(word, url)
                ]);
        }

        public MessageEntity SendAudio(int itemType, 
            int id,
            string url) {
            // file = FileRepository.UploadVideo(fieldKey);
            var word = "[语音]";
            return Send(itemType, id, environment.UserId,
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
                if (db.TrySave(message)) {
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
            List<MessageModel> data;
            Sql sql;
            if (startTime == 0) {
                sql = new Sql();
                sql.Select()
                    .From<MessageEntity>(db);
                if (itemType > 0)
                {
                    sql.Where("group_id=@0", id);
                }
                else
                {
                    sql.Where("group_id=0 and receive_id=@0",
                        environment.UserId, id);
                }
                data = db.Fetch<MessageModel>(sql.OrderBy("created_at desc").Limit(10));
                data.Reverse();
            } else {
                sql = new Sql();
                sql.Select()
                    .From<MessageEntity>(db)
                    .Where("created_at>=@0", startTime);

                if (itemType > 0)
                {
                    sql.Where("group_id=@0", id);
                }
                else
                {
                    sql.Where("group_id=0 and receive_id=@0",
                        environment.UserId, id);
                }
                data = db.Fetch<MessageModel>(sql.OrderBy("created_at asc"));
            }
            userStore.WithUser(data);
            WithReceive(data);
            var next_time = environment.Now + 1;
            if (data.Count == 0 || itemType == 0) {
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
            var sql = new Sql();
            sql.Select().From<GroupUserEntity>(db)
                .Where("group_id=@0", id)
                .WhereIn("user_id", ids);
            return db.Fetch<GroupUserEntity>(sql).ToDictionary(i => i.UserId);
        }

        /**
         * 消息撤回
         * @param int id
         * @throws \Exception
         */
        public void Revoke(int id) {
            var model = db.SingleById<MessageEntity>(id);
            if (model is null) {
                throw new Exception("消息错误");
            }
            if (model.UserId != environment.UserId) {
                throw new Exception("操作错误");
            }
            if (model.CreatedAt < environment.Now - 120) {
                throw new Exception("超过两分钟无法撤回");
            }
            db.DeleteById<MessageEntity>(id);
        }
        
    }
}
