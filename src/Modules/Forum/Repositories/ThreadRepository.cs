using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Forum.Entities;
using NetDream.Modules.Forum.Forms;
using NetDream.Modules.Forum.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Providers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NetDream.Modules.Forum.Repositories
{
    public class ThreadRepository(ForumContext db, 
        IUserRepository userStore,
        IClientContext client)
    {
        public IPage<ThreadModel> ManageList(string keywords = "", 
            int forum_id = 0, int page = 1)
        {
            var items = db.Threads.Search(keywords, "title")
                .When(forum_id > 0, i => i.ForumId == forum_id)
                .OrderByDescending(i => i.UpdatedAt)
                .ToPage(page).CopyTo<ThreadEntity, ThreadModel>();
            userStore.Include(items.Items);
            IncludeForum(items.Items);
            return items;
        }

        private void IncludeForum(IEnumerable<ThreadModel> items)
        {
            var idItems = items.Select(item => item.ForumId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Forums.Where(i => idItems.Contains(i.Id)).ToArray();
            if (data.Length == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.ForumId == it.Id)
                    {
                        item.Forum = it;
                        break;
                    }
                }
            }
        }
        private void IncludeClassify(IEnumerable<ThreadModel> items)
        {
            var idItems = items.Select(item => item.ClassifyId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = db.ForumClassifies.Where(i => idItems.Contains(i.Id)).ToArray();
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.ClassifyId == it.Id)
                    {
                        item.Classify = it;
                        break;
                    }
                }
            }
        }


        public ThreadEntity? Get(int id)
        {
            return db.Threads.Where(i => i.Id == id).Single();
        }

        public IOperationResult<ThreadModel> GetSource(int id)
        {
            var model = db.Threads.Where(i => i.Id == id && i.UserId == client.UserId).Single()?.CopyTo<ThreadModel>();
            if (model is null)
            {
                return OperationResult<ThreadModel>.Fail("操作失败");
            }
            model.Content = db.ThreadPosts.Where(i => i.UserId == client.UserId && i.ThreadId == model.Id && i.Grade == 0)
                .Select(i => i.Content).Single();
            return OperationResult.Ok(model);
        }

        public IOperationResult<ThreadEntity> Save(ThreadForm data)
        {
            var model = data.Id > 0 ? db.Threads.Where(i => i.Id == data.Id).Single() :
                new ThreadEntity();
            if (model is null) 
            {
                return OperationResult<ThreadEntity>.Fail("id is error");
            }
            model.Title = data.Title;
            model.ClassifyId = data.ClassifyId;
            model.ForumId = data.ForumId;
            if (model.UserId == 0)
            {
                model.UserId = client.UserId;
            }
            db.Threads.Save(model, client.Now);
            db.SaveChanges();
            if (data.Id == 0)
            {
                db.ThreadPosts.Add(new ThreadPostEntity()
                {
                    ThreadId = model.Id,
                    Content = data.Content,
                    UserId = client.UserId,
                    Grade = 0,
                    Ip = client.Ip,
                    CreatedAt = client.Now,
                    UpdatedAt = client.Now,
                });
            } else if (!string.IsNullOrWhiteSpace(data.Content))
            {
                db.ThreadPosts.Where(i => i.ThreadId == model.Id && i.Grade == 0)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Content, data.Content));

            }
            return OperationResult.Ok(model);
        }

        public void ManageRemove(params int[] id)
        {
            db.Threads.Where(i => id.Contains(i.Id)).ExecuteDelete();
            db.ThreadPosts.Where(i => id.Contains(i.ThreadId)).ExecuteDelete();
        }

        public IPage<ThreadModel> GetList(int forum, int classify = 0, 
            string keywords = "", int user = 0, int type = 0, 
            string sort = "", string order = "", int page = 1)
        {
            (sort, order) = SearchHelper.CheckSortOrder(sort, order, [
                "updated_at", "created_at", "post_count", "top_type"
            ]);

            var query = db.Threads.When(classify > 0, i => i.ClassifyId == classify)
                .Where(i => i.ForumId == forum);
            if (type < 2)
            {
                query = query.Search(keywords, "title");
            } else
            {
                var users = userStore.SearchUserId(keywords);
                if (users.Length == 0)
                {
                    return new Page<ThreadModel>();
                }
                query = query.Where(i => users.Contains(i.UserId));
            } 
            var items = query.When(user > 0, i => i.UserId == user)
                .OrderBy<ThreadEntity, int>(sort, order)
                .ToPage(page).CopyTo<ThreadEntity, ThreadModel>();
            userStore.Include(items.Items);
            IncludeClassify(items.Items);
            foreach (var item in items.Items)
            {
                item.LastPost = LastPost(item.Id);
                item.IsNew = IsNew(item);
            }
            return items;
        }

        public IPage<ThreadModel> SelfList(string keywords = "", 
            string sort = "", 
            string order = "", int page = 1)
        {
            (sort, order) = SearchHelper.CheckSortOrder(sort, order, [
                "updated_at", "created_at", "post_count", "top_type"
            ]);
            var items = db.Threads.Where(i => i.UserId == client.UserId)
                .Search(keywords, "title")
                .OrderBy<ThreadEntity, int>(sort, order)
                .ToPage(page).CopyTo<ThreadEntity, ThreadModel>();
            IncludeClassify(items.Items);
            IncludeForum(items.Items);
            return items;
        }

        public ThreadModel[] TopList(int forum)
        {
            var data = db.Threads.Where(i => i.ForumId == forum && i.TopType > 0)
                .OrderByDescending(i => i.TopType)
                .OrderByDescending(i => i.Id)
                .ToArray().CopyTo<ThreadEntity, ThreadModel>();
            userStore.Include(data);
            IncludeClassify(data);
            foreach (var item in data)
            {
                item.LastPost = LastPost(item.Id);
                item.IsNew = IsNew(item);
            }
            return data;
        }

        protected bool IsNew(ThreadModel model)
        {
            var time = model.CreatedAt;
            if (model.LastPost is not null)
            {
                time = model.LastPost.CreatedAt;
            }
            return time > client.Now - 86400;
        }

        protected ThreadPostModel? LastPost(int thread, 
            bool hasUser = true)
        {
            var data = db.ThreadPosts.Where(i => i.ThreadId == thread)
                .OrderByDescending(i => i.Id).Single()?.CopyTo<ThreadPostModel>();
            if (data is not null && hasUser)
            {
                data.User = userStore.Get(data.UserId);
            }
            return data;
        }

        public void PostList(int thread_id, 
            int user_id = 0, int post_id = 0, int status = 0, 
            string sort = "", 
            string order = "", int per_page = 20, 
            string type = "", int page = 1)
        {
            //page = -1;
            //if (post_id > 0)
            //{
            //    maps = ThreadPostModel.When(user_id > 0, use(user_id)(object query) {
            //        query.Where("user_id", user_id);
            //    })
            //    .Where("thread_id", thread_id)
            //    .OrderBy("grade", "asc")
            //    .OrderBy(MigrationTable.COLUMN_CREATED_AT, "asc").Pluck("id");
            //    count = empty(maps) ? false : array_search(post_id, maps);
            //    if (count === false)
            //    {
            //        throw new Exception("找不到该回帖");
            //    }
            //    page = (int)ceil((count + 1) / per_page);
            //}
            //list(sort, order) = SearchModel.CheckSortOrder(sort, order, [
            //    "grade", "status"
            //], "asc");
            ///** @var Page<ThreadPostModel> items */
            //items = ThreadPostModel.With("user", "thread")
            //    .When(user_id > 0, use(user_id)(object query) {
            //    query.Where("user_id", user_id);
            //})
            //.When(status > 0, use(status)(object query) {
            //    query.Where("status", status);
            //})
            //.Where("thread_id", thread_id)
            //.OrderBy(sort, order)
            //.OrderBy(MigrationTable.COLUMN_CREATED_AT, "asc")
            //.Page(per_page, "page", page);
            //data = items.GetPage();
            //foreach (data as item)
            //{
            //    item.IsPublicPost = item.GetIsPublicPostAttribute();
            //    item.Content = item.IsPublicPost ? Parser.Create(item, request())
            //        .Render(type) : string.Empty;
            //    item.Deleteable = static.CanRemovePost(item.Thread, item);
            //}
            //usort(data, (object a, object b) {
            //    if (a["grade"] < 1 || b["grade"] < 1)
            //    {
            //        return a["grade"] > b["grade"] ? 1 : -1;
            //    }
            //    return 0;
            //});
            //return items.SetPage(data);
        }

        public IPage<ThreadPostEntity> SelfPostList(string keywords = "", 
            int page = 1)
        {
            return db.ThreadPosts.Where(i => i.UserId == client.UserId)
                .Search(keywords, "content")
                .OrderBy(i => i.CreatedAt)
                .ToPage(page);
        }

        /**
         * 收藏
         * @param id
         * @return bool
         * @throws Exception
         */
        public IOperationResult<bool> ToggleCollect(int id)
        {
            var model = db.Threads.Where(i => i.Id == id).Single();
            if (model is null)
            {
                return OperationResult<bool>.Fail("帖子不存在");
            }
            return OperationResult.Ok(ToggleCollectThread(model));
        }

        public bool ToggleCollectThread(ThreadEntity model)
        {
            var yes = new LogRepository(db)
                .ToggleAction(client.UserId, model.Id, LogRepository.TYPE_THREAD, 
                LogRepository.ACTION_COLLECT);
            model.CollectCount += (yes ? 1 : -1);
            db.Update(model);
            return yes;
        }

        /**
         * 是否同意回帖内容
         * @param id
         * @param bool agree
         * @return array
         * @throws Exception
         */
        public IOperationResult<AgreeResult> AgreePost(int id, bool agree = true)
        {
            var model = db.ThreadPosts.Where(i => i.Id == id).Single();
            if (model is null)
            {
                return OperationResult<AgreeResult>.Fail("回复不存在");
            }
            var action = agree ? LogRepository.ACTION_AGREE : LogRepository.ACTION_DISAGREE;
            var res = new LogRepository(db).ToggleLog(
                client.UserId,
                model.Id,
                LogRepository.TYPE_THREAD_POST,
                action,
                [LogRepository.ACTION_AGREE, LogRepository.ACTION_DISAGREE]);
            if (res < 1)
            {
                if (action == LogRepository.ACTION_AGREE)
                {
                    model.AgreeCount--;
                }
                else
                {
                    model.DisagreeCount--;
                }
            }
            else if(res == 1) {
                var plus = action == LogRepository.ACTION_AGREE ? 1 : -1;
                model.AgreeCount += plus;
                model.DisagreeCount -= plus;
            } else
            {
                if (action == LogRepository.ACTION_AGREE)
                {
                    model.AgreeCount++;
                }
                else
                {
                    model.DisagreeCount++;
                }
            }
            db.Update(model);
            return OperationResult.Ok(new AgreeResult(agree, model.AgreeCount, model.DisagreeCount));
        }

        public IOperationResult<ThreadPostEntity> Create(string title, 
            string content, int forum_id, int classify_id = 0, 
            byte is_private_post = 0)
        {
            title = title.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                return OperationResult<ThreadPostEntity>.Fail("标题不能为空");
            }
            if (forum_id < 1)
            {
                return OperationResult<ThreadPostEntity>.Fail("请选择版块");
            }
            var userId = client.UserId;
            if (!CanPublish(userId, title)) {
                return OperationResult<ThreadPostEntity>.Fail("你的操作太频繁了，请五分钟后再试");
            }
            var thread = new ThreadEntity()
            {
                Title = title,
                ForumId = forum_id,
                ClassifyId = classify_id,
                UserId = userId,
                IsPrivatePost = is_private_post
            };
            db.Threads.Save(thread, client.Now);
            if (db.SaveChanges() == 0)
            {
                return OperationResult<ThreadPostEntity>.Fail("发帖失败");
            }
            var model = new ThreadPostEntity() {
                Content = content,
                UserId=userId,
                ThreadId = thread.Id,
                Grade =0,
                Ip = client.Ip,
            };
            db.ThreadPosts.Save(model, client.Now);
            new ForumRepository(db, userStore).
                UpdateCount(thread.ForumId, "thread_count");
            return OperationResult.Ok(model);
        }

        public bool CanPublish(int userId, string title)
        {
            var now = TimeHelper.TimestampNow();
            var count = db.Threads.Where(i => i.UserId == userId && i.CreatedAt > now - 300)
                .Any();
            if (count)
            {
                return false;
            }
            count = db.Threads.Where(i => i.UserId == userId && i.CreatedAt > now - 3600 && i.Title == title).Any();
            return !count;
        }

        public IOperationResult<ThreadEntity> Update(int id, ThreadForm data)
        {
            var thread = Get(id);
            if (thread is null)
            {
                return OperationResult<ThreadEntity>.Fail("无权限");
            }
            if (thread.UserId != client.UserId)
            {
                return OperationResult<ThreadEntity>.Fail("无权限");
            }
            if (thread.IsClosed > 0)
            {
                return OperationResult<ThreadEntity>.Fail("帖子已锁定，无法编辑");
            }
            if (!string.IsNullOrWhiteSpace(data.Title))
            {
                thread.Title = data.Title;
            }
            if (data.ClassifyId > 0)
            {
                thread.ClassifyId = data.ClassifyId;
            }
            if (data.IsPrivatePost > 0)
            {
                thread.IsPrivatePost = data.IsPrivatePost;
            }
            db.Threads.Save(thread, client.Now);
            db.ThreadPosts.Where(i => i.ThreadId == thread.Id && i.Grade == 0 && i.UserId == client.UserId)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Content, data.Content));
            return OperationResult.Ok(thread);
        }


        public IOperationResult<ThreadModel> GetFull(int id, bool isSee = false)
        {
            var res = db.Threads.Where(i => i.Id == id).Single();
            if (res is null)
            {
                return OperationResult<ThreadModel>.Fail("id is error");
            }
            if (isSee)
            {
                res.ViewCount++;
                db.SaveChanges();
            }
            var model = res.CopyTo<ThreadModel>();

            //model.Forum;
            //model.Path = array_merge(ForumModel.FindPath(model.ForumId), [model.Forum]);
            //model.Digestable = static.Can(model, "is_digest");
            //model.Highlightable = static.Can(model, "is_highlight");
            //model.Closeable = static.Can(model, "is_closed");
            //model.Topable = static.Can(model, "top_type");
            //model.Editable = static.Editable(model);
            //model.Classify;
            //model.LastPost = static.LastPost(model.Id, false);
            //model.IsNew = static.IsNew(model);
            //model.LikeType = LogRepository.UserActionValue(id, ThreadLogModel.TYPE_THREAD,
            //    [ThreadLogModel.ACTION_AGREE, ThreadLogModel.ACTION_DISAGREE]);
            //model.IsCollected = LogRepository.UserAction(id, ThreadLogModel.TYPE_THREAD,
            //    ThreadLogModel.ACTION_COLLECT);
            //model.IsReward = LogRepository.UserAction(id, ThreadLogModel.TYPE_THREAD,
            //    ThreadLogModel.ACTION_REWARD);
            //model.RewardCount = ThreadLogModel.Where("item_type", ThreadLogModel.TYPE_THREAD)
            //    .Where("item_id", id)
            //    .Where("action", ThreadLogModel.ACTION_REWARD).Count();
            //model.RewardItems = ThreadLogModel.With("user")
            //    .Where("item_type", ThreadLogModel.TYPE_THREAD)
            //    .Where("item_id", id)
            //    .Where("action", ThreadLogModel.ACTION_REWARD).OrderBy("id", "desc")
            //    .Limit(5).Get();
            return OperationResult.Ok(model);
        }

        public bool Editable(ThreadEntity model)
        {
            if (client.UserId == 0 || model.IsClosed > 0)
            {
                return false;
            }
            return model.UserId == client.UserId || Can(model, "edit");
        }

        public IOperationResult<ThreadPostEntity> Reply(string content, int thread_id)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return OperationResult<ThreadPostEntity>.Fail("请输入内容");
            }
            if (thread_id < 1)
            {
                return OperationResult<ThreadPostEntity>.Fail("请选择帖子");
            }
            var thread = Get(thread_id);
            if (thread is null || thread.IsClosed > 0)
            {
                return OperationResult<ThreadPostEntity>.Fail("帖子已关闭");
            }
            var max = db.ThreadPosts.Where(i => i.ThreadId == thread_id).Max(i => i.Grade);
            var post = new ThreadPostEntity() {
                Content = content,
                UserId = client.UserId,
                ThreadId = thread_id,
                Grade = max + 1,
                Ip = client.Ip,
            };
            db.ThreadPosts.Save(post, client.Now);
            if (db.SaveChanges() == 0)
            {
                return OperationResult<ThreadPostEntity>.Fail("发表失败");
            }
            new ForumRepository(db, userStore)
                .UpdateCount(thread.ForumId, "post_count");
            db.Threads.Where(i => i.Id == thread_id)
                .ExecuteUpdate(setters => 
                    setters.SetProperty(i => i.PostCount, i => i.PostCount + 1)
                    .SetProperty(i => i.UpdatedAt, client.Now));
            return OperationResult.Ok(post);
        }

        /**
         * 操作主题
         * @param int id
         * @return ThreadModel
         * @throws Exception
         */
        public IOperationResult<ThreadEntity> ThreadAction(int id, Dictionary<string, object> data)
        {
            var thread = Get(id);
            if (thread is null)
            {
                return OperationResult<ThreadEntity>.Fail("请选择帖子");
            }
            //if (in_array("like", data) || array_key_exists("like", data))
            //{
            //    thread.LikeType = static.ToggleLike(thread);
            //    return thread;
            //}
            //if (in_array("collect", data) || array_key_exists("collect", data))
            //{
            //    thread.IsCollected = static.ToggleCollectThread(thread);
            //    return thread;
            //}
            //if (isset(data["reward"]))
            //{
            //    static.RewardThread(thread, floatval(data["reward"]));
            //    thread.IsReward = true;
            //    return thread;
            //}
            //maps = ["is_highlight", "is_digest", "is_closed", "top_type"];
            //foreach (data as action => val)
            //{
            //    if (is_int(action))
            //    {
            //        if (empty(val))
            //        {
            //            continue;
            //        }
            //        list(action, val) = [val, thread.{ val} > 0 ? 0 : 1];
            //    }
            //    if (empty(action) || !in_array(action, maps))
            //    {
            //        continue;
            //    }
            //    if (!Can(thread, action)) {
            //        throw new Exception("无权限");
            //    }
            //    thread.{ action} = intval(val);
            //}
            //thread.Save();
            //ForumLogModel.Create([
            //    "item_type" => ForumLogModel.TYPE_THREAD,
            //    "item_id" => thread.Id,
            //    "action" => 1,
            //    "data" => Json.Encode(data)
            //]);
            return OperationResult.Ok(thread);
        }

        /**
         * 是否有权限执行操作
         * @param ThreadModel model
         * @param string action
         * @return bool
         * @throws Exception
         */
        public bool Can(ThreadEntity model, string action)
        {
            if (client.UserId == 0)
            {
                return false;
            }
            return true;// TODO auth().User().HasRole("administrator");
        }

        public bool CanRemovePost(ThreadEntity model, ThreadPostEntity item)
        {
            if (client.UserId == 0)
            {
                return false;
            }
            if (client.UserId == model.UserId)
            {
                return true;
            }
            if (client.UserId == item.UserId)
            {
                return true;
            }
            return true;//TODO auth().User().HasRole("administrator");
        }

        public IOperationResult RemovePost(int id)
        {
            var item = db.ThreadPosts.Where(i => i.Id == id).Single();
            if (item is null)
            {
                return OperationResult.Fail("请选择回帖");
            }
            var thread = Get(item.ThreadId);
            if (thread is null)
            {
                return OperationResult.Fail("请选择回帖");
            }
            if (!CanRemovePost(thread, item)) {
                return OperationResult.Fail("无权限");
            }
            db.ThreadPosts.Remove(item);
            db.SaveChanges();
            new ForumRepository(db, userStore).UpdateCount(thread.ForumId, "post_count", -1);
            db.Threads.Where(i => i.Id == item.ThreadId)
                .ExecuteUpdate(setters =>
                    setters.SetProperty(i => i.PostCount, i => i.PostCount - 1)
                    .SetProperty(i => i.UpdatedAt, client.Now));
            return OperationResult.Ok();
        }

        public IOperationResult Remove(int id)
        {
            var thread = Get(id);
            if (thread is null || (thread.UserId != client.UserId && Can(thread, "delete"))) {
                return OperationResult.Fail("操作失败");
            }
            db.Threads.Remove(thread);
            var count = db.ThreadPosts.Where(i => i.ThreadId == id).Count() - 1;
            db.ThreadPosts.Where(i => i.ThreadId == id).ExecuteDelete();
            var repository = new ForumRepository(db, userStore);
            repository.UpdateCount(thread.ForumId, "thread_count", -1);
            repository.UpdateCount(thread.ForumId, "post_count", -count);
            db.Logs.Add(new LogEntity() { 
                ItemType = LogRepository.TYPE_THREAD,
                ItemId = id,
                Action = LogRepository.ACTION_DELETE,
                UserId = client.UserId,
                CreatedAt = client.Now,
            });
            db.SaveChanges();
            return OperationResult.Ok();
        }


        public ThreadEntity[] Suggestion(string keywords = "")
        {
            return db.Threads.Search(keywords, "title")
                .Take(4).Select(i => new ThreadEntity()
                {
                    Id = i.Id,
                    Title = i.Title,
                }).ToArray();
        }

        public IPage<ThreadLogModel> RewardList(int item_id, 
            byte item_type = 0, int page = 1)
        {
            var items = db.ThreadLogs.Where(i => i.ItemType == item_type && i.ItemId == item_id && i.Action == LogRepository.ACTION_REWARD)
                .OrderByDescending(i => i.Id).ToPage(page).CopyTo<ThreadLogEntity, ThreadLogModel>();
            userStore.Include(items.Items);
            return items;
        }

        private byte ToggleLike(ThreadEntity thread)
        {
            return new LogRepository(db).ToggleLog(client.UserId,
                thread.Id,
                LogRepository.TYPE_THREAD,
                LogRepository.ACTION_AGREE, [
                LogRepository.ACTION_AGREE, LogRepository.ACTION_DISAGREE
            ]);
        }

        private void RewardThread(ThreadModel thread, float money)
        {
            if (thread.UserId == client.UserId)
            {
                throw new Exception("不能自己打赏自己");
            }
            //res = FundAccount.PayTo(auth().Id(),
            //    FundAccount.TYPE_FORUM_BUY, use(thread)() {
            //    return ThreadLogModel.Create([
            //        "item_type" => ThreadLogModel.TYPE_THREAD,
            //    "item_id" => thread.Id,
            //    "action" => ThreadLogModel.ACTION_REWARD,
            //    "user_id" => auth().Id(),
            //]);
            //}, -money, sprintf("打赏帖子《%s》", thread.Title), thread.UserId);
            //if (!res)
            //{
            //    throw new Exception("支付失败，请检查您的账户余额");
            //}
        }

        public IOperationResult<ThreadPostEntity> ChangePost(int id, byte status)
        {
            var model = db.ThreadPosts.Where(i => i.Id == id).Single();
            if (model is null)
            {
                return OperationResult<ThreadPostEntity>.Fail("id is error");
            }
            var thread = db.Threads.Where(i => i.Id == model.ThreadId).Single();
            if (!Editable(thread)) {
                return OperationResult<ThreadPostEntity>.Fail("无权限操作");
            }
            model.Status = status;
            db.ThreadPosts.Save(model, client.Now);
            db.Logs.Add(new LogEntity()
            {
                ItemType = LogRepository.TYPE_POST,
                ItemId = id,
                Action = LogRepository.ACTION_STATUS,
                Data = status.ToString(),
                UserId = client.UserId,
                CreatedAt = client.Now,
            });
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /**
         * 获取用户的统计信息
         * @param int id
         * @return array
         */
        //public static array GetUser(int id)
        //{
        //    user = UserRepository.GetPublicProfile(id, "card_items");
        //    user["count_items"] = [
        //        ["name" => "主题", "count" => ThreadModel.Where("user_id", id).Count()],
        //    ["name" => "帖子", "count" => ThreadPostModel.Where("user_id", id).Count()],
        //    ["name" => "积分", "count" => 0],
        //];
        //    user["medal_items"] = [
        //    // ["name" => string.Empty, "icon" => url().Asset(string.Empty)],
        //    ];
        //    return user;
        //}
    }
}
