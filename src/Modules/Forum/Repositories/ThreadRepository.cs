using MediatR;
using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Forum.Entities;
using NetDream.Modules.Forum.Forms;
using NetDream.Modules.Forum.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Notifications;
using NetDream.Shared.Providers;
using NetDream.Shared.Providers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NetDream.Modules.Forum.Repositories
{
    public class ThreadRepository(ForumContext db, 
        IUserRepository userStore,
        IClientContext client, IMediator mediator)
    {
        public IPage<ThreadModel> ManageList(ThreadQueryForm form)
        {
            var items = db.Threads.Search(form.Keywords, "title")
                .When(form.Forum > 0, i => i.ForumId == form.Forum)
                .OrderByDescending(i => i.UpdatedAt)
                .ToPage(form).CopyTo<ThreadEntity, ThreadModel>();
            userStore.Include(items.Items);
            IncludeForum(items.Items);
            return items;
        }

        private void IncludeForum(IEnumerable<IWithForumModel> items)
        {
            var idItems = items.Select(item => item.ForumId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Forums.Where(i => idItems.Contains(i.Id))
                .Select(i => new ForumLabelItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Thumb = i.Thumb,
                }).ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (data.TryGetValue(item.ForumId, out var it))
                {
                    item.Forum = it;
                }
            }
        }
        private void IncludeClassify(IEnumerable<IWithClassifyModel> items)
        {
            var idItems = items.Select(item => item.ClassifyId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = db.ForumClassifies.Where(i => idItems.Contains(i.Id)).ToDictionary(i => i.Id);
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                if (data.TryGetValue(item.ClassifyId, out var it))
                {
                    item.Classify = it;
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

        public IPage<ThreadListItem> GetList(ThreadQueryForm form)
        {
            var (sort, order) = SearchHelper.CheckSortOrder(form.Sort, form.Order, [
                "updated_at", "created_at", "post_count", "top_type"
            ]);

            var query = db.Threads.When(form.Classify > 0, i => i.ClassifyId == form.Classify)
                .Where(i => i.ForumId == form.Forum);
            if (form.Type < 2)
            {
                query = query.Search(form.Keywords, "title");
            } else
            {
                var users = userStore.SearchUserId(form.Keywords);
                if (users.Length == 0)
                {
                    return new Page<ThreadListItem>();
                }
                query = query.Where(i => users.Contains(i.UserId));
            } 
            var items = query.When(form.User > 0, i => i.UserId == form.User)
                .OrderBy<ThreadEntity, int>(sort, order)
                .ToPage(form).CopyTo<ThreadEntity, ThreadListItem>();
            if (items.Items.Length == 0)
            {
                return items;
            }
            userStore.Include(items.Items);
            IncludeClassify(items.Items);
            var posts = GetMainPost(items.Items.Select(i => i.Id));
            foreach (var item in items.Items)
            {
                // item.LastPost = LastPost(item.Id);
                item.IsNew = IsNew(item);
                if (posts.TryGetValue(item.Id, out var post))
                {
                    item.AgreeCount = post.AgreeCount;
                    item.DisagreeCount = post.DisagreeCount;
                    item.Brief = post.Content;
                }
            }
            return items;
        }

        private IDictionary<int, ThreadPostEntity> GetMainPost(IEnumerable<int> threadItems)
        {
            return db.ThreadPosts.Where(i => threadItems.Contains(i.ThreadId) && i.Grade == 0)
                .ToDictionary(i => i.ThreadId);
        }

        public IPage<ThreadListItem> SelfList(ThreadQueryForm form)
        {
            var (sort, order) = SearchHelper.CheckSortOrder(form.Sort, form.Order, [
                "updated_at", "created_at", "post_count", "top_type"
            ]);
            var items = db.Threads.Where(i => i.UserId == client.UserId)
                .Search(form.Keywords, "title")
                .OrderBy<ThreadEntity, int>(sort, order)
                .ToPage(form).CopyTo<ThreadEntity, ThreadListItem>();
            IncludeClassify(items.Items);
            IncludeForum(items.Items);
            return items;
        }
        public ThreadModel[] TopList(int forum)
        {
            var data = db.Threads.Where(i => i.ForumId == forum && i.TopType > 0)
                .OrderByDescending(i => i.TopType)
                .ThenByDescending(i => i.Id)
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

        protected bool IsNew(ThreadEntity model)
        {
            var time = model.UpdatedAt;
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

        public IPage<PostListItem> PostList(PostQueryForm form)
        {
            if (form.Post > 0)
            {
                var maps = db.ThreadPosts.When(form.User > 0, i => i.UserId == form.User)
                    .Where(i => i.ThreadId == form.Thread)
                    .OrderBy(i => i.Grade)
                    .ThenBy(i => i.CreatedAt).Pluck(i => i.Id);
                var index = maps.Length == 0 ? -1 : Array.IndexOf(maps, form.Post);
                if (index < 0)
                {
                    throw new Exception("找不到该回帖");
                }
                form.Page = (int)Math.Ceiling((double)(index + 1) / form.PerPage);
            }
            var (sort, order) = SearchHelper.CheckSortOrder(form.Sort, form.Order, [
                "grade", "status"
            ], "asc");
            var res = db.ThreadPosts
                .When(form.User > 0, i => i.UserId == form.User)
                .When(form.Status > 0, i => i.Status == form.Status)
                .Where(i => i.ThreadId == form.Thread)
                .OrderBy<ThreadPostEntity, int>(sort, order)
                .ThenBy(i => i.CreatedAt)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(res.Items);
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
            return res;
        }

        public IPage<ThreadPostEntity> SelfPostList(QueryForm form)
        {
            return db.ThreadPosts.Where(i => i.UserId == client.UserId)
                .Search(form.Keywords, "content")
                .OrderBy(i => i.CreatedAt)
                .ToPage(form);
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

        /// <summary>
        /// 是否同意回帖内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="agree"></param>
        /// <returns></returns>
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

        public IOperationResult<ThreadPostEntity> Create(ThreadPublishForm form)
        {
            if (string.IsNullOrWhiteSpace(form.Title))
            {
                return OperationResult<ThreadPostEntity>.Fail("标题不能为空");
            }
            if (form.Forum < 1)
            {
                return OperationResult<ThreadPostEntity>.Fail("请选择版块");
            }
            var userId = client.UserId;
            if (!CanPublish(userId, form.Title)) {
                return OperationResult<ThreadPostEntity>.Fail("你的操作太频繁了，请五分钟后再试");
            }
            var thread = new ThreadEntity()
            {
                Title = form.Title,
                ForumId = form.Forum,
                ClassifyId = form.Classify,
                UserId = userId,
                IsPrivatePost = form.IsPrivatePost
            };
            db.Threads.Save(thread, client.Now);
            if (db.SaveChanges() == 0)
            {
                return OperationResult<ThreadPostEntity>.Fail("发帖失败");
            }
            var model = new ThreadPostEntity() {
                Content = form.Content,
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

        public IOperationResult<ThreadEntity> Update(ThreadForm data)
        {
            var thread = Get(data.Id);
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

        public IOperationResult<ThreadPostEntity> Reply(ThreadReplyForm form)
        {
            if (string.IsNullOrWhiteSpace(form.Content))
            {
                return OperationResult<ThreadPostEntity>.Fail("请输入内容");
            }
            if (form.Thread < 1)
            {
                return OperationResult<ThreadPostEntity>.Fail("请选择帖子");
            }
            var thread = Get(form.Thread);
            if (thread is null || thread.IsClosed > 0)
            {
                return OperationResult<ThreadPostEntity>.Fail("帖子已关闭");
            }
            var max = db.ThreadPosts.Where(i => i.ThreadId == form.Thread).Max(i => i.Grade);
            var post = new ThreadPostEntity() {
                Content = form.Content,
                UserId = client.UserId,
                ThreadId = form.Thread,
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
            db.Threads.Where(i => i.Id == form.Thread)
                .ExecuteUpdate(setters => 
                    setters.SetProperty(i => i.PostCount, i => i.PostCount + 1)
                    .SetProperty(i => i.UpdatedAt, client.Now));
            return OperationResult.Ok(post);
        }


        public IOperationResult<ThreadEntity> ThreadAction(int id, string[] data)
        {
            var thread = db.Threads.Where(i => i.Id == id).Single();
            if (thread is null)
            {
                return OperationResult<ThreadEntity>.Fail("请选择帖子");
            }
            var res = thread.CopyTo<ThreadModel>();
            if (data.Contains("like"))
            {
                res.LikeType = ToggleLike(thread);
                return OperationResult.Ok<ThreadEntity>(res);
            }
            if (data.Contains("collect"))
            {
                res.IsCollected = ToggleCollectThread(thread);
                return OperationResult.Ok<ThreadEntity>(res);
            }
            var type = thread.GetType();
            foreach (var action in data)
            {
                if (!(action is "is_highlight" or "is_digest" or "is_closed" or "top_type"))
                {
                    continue;
                }
                if (Can(thread, action))
                {
                    return OperationResult.Fail<ThreadEntity>("权限不足");
                }
                var field = type.GetField(StrHelper.Studly(action));
                if (field is null)
                {
                    continue;
                }
                field.SetValue(thread, (byte)((byte)field.GetValue(thread) > 0 ? 0 : 1));
            }
            db.Threads.Save(thread, client.Now);
            db.Logs.Add(new LogEntity()
            {
                ItemType = LogRepository.TYPE_THREAD,
                ItemId = thread.Id,
                Action = 1,
                Data = JsonSerializer.Serialize(data),
                UserId = client.UserId,
                CreatedAt = client.Now,
            });
            db.SaveChanges();
            return OperationResult.Ok(thread);
        }
        /// <summary>
        /// 操作主题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public IOperationResult<ThreadEntity> ThreadAction(int id, Dictionary<string, int> data)
        {
            var thread = Get(id);
            if (thread is null)
            {
                return OperationResult<ThreadEntity>.Fail("请选择帖子");
            }
            var res = thread.CopyTo<ThreadModel>();
            if (data.ContainsKey("like"))
            {
                res.LikeType = ToggleLike(thread);
                return OperationResult.Ok<ThreadEntity>(res);
            }
            if (data.ContainsKey("collect"))
            {
                res.IsCollected = ToggleCollectThread(thread);
                return OperationResult.Ok<ThreadEntity>(res);
            }
            if (data.TryGetValue("reward", out var val))
            {
                RewardThread(thread, val);
                res.IsReward = true;
                return OperationResult.Ok<ThreadEntity>(res);
            }
            var type = thread.GetType();
            foreach (var action in data)
            {
                if (!(action.Key is "is_highlight" or "is_digest" or "is_closed" or "top_type"))
                {
                    continue;
                }
                if (Can(thread, action.Key))
                {
                    return OperationResult.Fail<ThreadEntity>("权限不足");
                }
                var field = type.GetField(StrHelper.Studly(action.Key));
                if (field is null)
                {
                    continue;
                }
                field.SetValue(thread, (byte)(action.Value > 0 ? 1 : 0));
            }
            db.Threads.Save(thread, client.Now);
            db.Logs.Add(new LogEntity()
            {
                ItemType = LogRepository.TYPE_THREAD,
                ItemId = thread.Id,
                Action = 1,
                Data = JsonSerializer.Serialize(data),
                UserId = client.UserId,
                CreatedAt = client.Now,
            });
            db.SaveChanges();
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
            return userStore.IsRole(client.UserId, "administrator");
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
            return userStore.IsRole(client.UserId, "administrator");
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


        public ListArticleItem[] Suggestion(string keywords = "")
        {
            return db.Threads.Search(keywords, "title")
                .Take(4).Select(i => new ListArticleItem()
                {
                    Id = i.Id,
                    Title = i.Title,
                }).ToArray();
        }

        public IPage<ThreadLogModel> RewardList(RewardQueryForm form)
        {
            var items = db.ThreadLogs.Where(i => i.ItemType == form.Type && i.ItemId == form.Id && i.Action == LogRepository.ACTION_REWARD)
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
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

        private void RewardThread(ThreadEntity thread, float money)
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

        /// <summary>
        /// 获取用户的统计信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IOperationResult<UserProfileCard> GetUser(int id)
        {
            var request = new UserProfileCardRequest(id, ["card_items"]);
            request.Add(
                new StatisticsItem("主题", db.Threads.Where(i => i.UserId == id).Count()),
                new StatisticsItem("帖子", db.ThreadPosts.Where(i => i.UserId == id).Count())
            );
            mediator.Publish(request).GetAwaiter().GetResult();
            return OperationResult.Ok(request.Result);
        }
    }
}
