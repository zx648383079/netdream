using Modules.Forum.Entities;
using NetDream.Modules.Forum.Forms;
using NetDream.Modules.Forum.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Shared.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Forum.Repositories
{
    public class ThreadRepository(IDatabase db, 
        IUserRepository userStore,
        IClientContext environment)
    {
        public Page<ThreadModel> ManageList(string keywords = "", 
            int forum_id = 0, int page = 1)
        {
            var sql = new Sql();
            sql.Select("*").From<ThreadEntity>(db);
            if (forum_id > 0)
            {
                sql.Where("forum_id=@0", forum_id);
            }
            SearchHelper.Where(sql, "title", keywords)
                .OrderBy("updated_at DESC");
            var items = db.Page<ThreadModel>(page, 20, sql);
            userStore.WithUser(items.Items);
            WithForum(items.Items);
            return items;
        }

        private void WithForum(IEnumerable<ThreadModel> items)
        {
            var idItems = items.Select(item => item.ForumId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Fetch<ForumEntity>($"WHERE id IN({string.Join(',', idItems)})");
            if (!data.Any())
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
        private void WithClassify(IEnumerable<ThreadModel> items)
        {
            var idItems = items.Select(item => item.ClassifyId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Fetch<ForumClassifyEntity>($"WHERE id IN({string.Join(',', idItems)})");
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
            return db.SingleById<ThreadEntity>(id);
        }

        public ThreadModel GetSource(int id)
        {
            var model = db.SingleById<ThreadModel>(id);
            if (model is null ||  model.UserId != environment.UserId)
            {
                throw new Exception("操作失败");
            }
            model.Content = db.ExecuteScalar<string>(new Sql().Select("content")
                .From<ThreadPostEntity>(db)
                .Where("user_id=@0 AND thread_id=@1 AND grade=0", environment.UserId, model.Id));
            return model;
        }

        public ThreadEntity Save(ThreadForm data)
        {
            var model = data.Id > 0 ? db.SingleById<ThreadEntity>(data.Id) :
                new ThreadEntity();
            model.Title = data.Title;
            model.ClassifyId = data.ClassifyId;
            model.ForumId = data.ForumId;
            if (model.UserId == 0)
            {
                model.UserId = environment.UserId;
            }
            db.TrySave(model);
            if (data.Id == 0)
            {
                db.Insert(new ThreadPostEntity()
                {
                    ThreadId = model.Id,
                    Content = data.Content,
                    UserId = environment.UserId,
                    Grade = 0,
                    Ip = environment.Ip,
                    CreatedAt = environment.Now,
                    UpdatedAt = environment.Now,
                });
            } else if (!string.IsNullOrWhiteSpace(data.Content))
            {
                db.Update<ThreadPostEntity>(new Sql().Where("thread_id=@0 AND grade=0", model.Id), new Dictionary<string, object>()
                {
                    { "content" , data.Content}
                });
            }
            return model;
        }

        public void ManageRemove(params int[] id)
        {
            db.DeleteById<ThreadEntity>(id);
            db.DeleteById<ThreadPostEntity>("thread_id", id);
        }

        public Page<ThreadModel> GetList(int forum, int classify = 0, 
            string keywords = "", int user = 0, int type = 0, 
            string sort = "", string order = "", int page = 1)
        {
            (sort, order) = SearchHelper.CheckSortOrder(sort, order, [
                MigrationTable.COLUMN_UPDATED_AT, MigrationTable.COLUMN_CREATED_AT, "post_count", "top_type"
            ]);

            var sql = new Sql();
            sql.Select("*").From<ThreadEntity>(db);
            if (classify > 0)
            {
                sql.Where("classify_id=@0", classify);
            }
            sql.WhereIn("forum_id", forum);
            if (type < 2)
            {
                SearchHelper.Where(sql, "title", keywords);
            } else
            {
                var users = userStore.SearchUserId(keywords);
                if (users.Length == 0)
                {
                    return new Page<ThreadModel>();
                }
                sql.WhereIn("user_id", users);
            } 
            if (user > 0)
            {
                sql.Where("user_id=@0", user);
            }
            sql.OrderBy($"{sort} {order}");
            var items = db.Page<ThreadModel>(page, 20, sql);
            userStore.WithUser(items.Items);
            WithClassify(items.Items);
            foreach (var item in items.Items)
            {
                item.LastPost = LastPost(item.Id);
                item.IsNew = IsNew(item);
            }
            return items;
        }

        public Page<ThreadModel> SelfList(string keywords = "", 
            string sort = "", 
            string order = "", int page = 1)
        {
            (sort, order) = SearchHelper.CheckSortOrder(sort, order, [
                MigrationTable.COLUMN_UPDATED_AT, MigrationTable.COLUMN_CREATED_AT, "post_count", "top_type"
            ]);
            var sql = new Sql().Select("*")
                .From<ThreadEntity>(db)
                .Where("user_id=@0", environment.UserId);
            SearchHelper.Where(sql, "title", keywords);
            sql.OrderBy($"{sort} {order}");
            var items = db.Page<ThreadModel>(page, 20, sql);
            WithClassify(items.Items);
            WithForum(items.Items);
            return items;
        }

        public IList<ThreadModel> TopList(int forum)
        {
            var data = db.Fetch<ThreadModel>(
                "WHERE forum_id=@0 AND top_type>0 ORDER BY top_type DESC, id DESC");
            userStore.WithUser(data);
            WithClassify(data);
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
            return time > environment.Now - 86400;
        }

        protected ThreadPostEntity LastPost(int thread, 
            bool hasUser = true)
        {
            var data = db.Single<ThreadPostModel>("WHERE thread_id=@0 ORDER BY id DESC", thread);
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

        public Page<ThreadPostModel> SelfPostList(string keywords = "", 
            int page = 1)
        {
            var sql = new Sql();
            sql.Select("*").From<ThreadPostEntity>(db)
                .Where("user_id", environment.UserId);
            SearchHelper.Where(sql, "content", keywords);
            sql.OrderBy("created_at ASC");
            return db.Page<ThreadPostModel>(page, 20, sql);
        }

        /**
         * 收藏
         * @param id
         * @return bool
         * @throws Exception
         */
        public bool ToggleCollect(int id)
        {
            var model = db.SingleById<ThreadEntity>(id);
            if (model is null)
            {
                throw new Exception("帖子不存在");
            }
            return ToggleCollectThread(model);
        }

        public bool ToggleCollectThread(ThreadEntity model)
        {
            var yes = new LogRepository(db)
                .ToggleAction(environment.UserId, model.Id, LogRepository.TYPE_THREAD, 
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
        public AgreeResult AgreePost(int id, bool agree = true)
        {
            var model = db.SingleById<ThreadPostEntity>(id);
            if (model is null)
            {
                throw new Exception("回复不存在");
            }
            var action = agree ? LogRepository.ACTION_AGREE : LogRepository.ACTION_DISAGREE;
            var res = new LogRepository(db).ToggleLog(
                environment.UserId,
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
            return new AgreeResult(agree, model.AgreeCount, model.DisagreeCount);
        }

        public ThreadPostEntity Create(string title, 
            string content, int forum_id, int classify_id = 0, 
            byte is_private_post = 0)
        {
            title = title.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new Exception("标题不能为空");
            }
            if (forum_id < 1)
            {
                throw new Exception("请选择版块");
            }
            var userId = environment.UserId;
            if (!CanPublish(userId, title)) {
                throw new Exception("你的操作太频繁了，请五分钟后再试");
            }
            var thread = new ThreadEntity()
            {
                Title = title,
                ForumId = forum_id,
                ClassifyId = classify_id,
                UserId = userId,
                IsPrivatePost = is_private_post
            };
            if (!db.TrySave(thread))
            {
                throw new Exception("发帖失败");
            }
            var model = new ThreadPostEntity() {
                Content = content,
                UserId=userId,
                ThreadId = thread.Id,
                Grade =0,
                Ip = environment.Ip,
            };
            db.TrySave(model);
            new ForumRepository(db, userStore).
                UpdateCount(thread.ForumId, "thread_count");
            return model;
        }

        public bool CanPublish(int userId, string title)
        {
            var count = db.FindCount<ThreadEntity>("user_id=@0 AND created_at>@1", userId,
                TimeHelper.TimestampNow() - 300);
            if (count > 0)
            {
                return false;
            }
            count = db.FindCount<ThreadEntity>("user_id=@0 AND created_at>@1 AND title=@2", userId,
                TimeHelper.TimestampNow() - 3600, title);
            return count < 1;
        }

        public ThreadEntity Update(int id, ThreadForm data)
        {
            var thread = Get(id);
            if (thread is null)
            {
                throw new Exception("无权限");
            }
            if (thread.UserId != environment.UserId)
            {
                throw new Exception("无权限");
            }
            if (thread.IsClosed > 0)
            {
                throw new Exception("帖子已锁定，无法编辑");
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
            db.TrySave(thread);
            db.Update<ThreadPostEntity>(new Sql().Where("user_id=@0 AND thread_id=@1 AND grade=0", environment.UserId,
                thread.Id), new Dictionary<string, object>()
                {
                    {"content", data.Content},
                });
            return thread;
        }


        public ThreadModel GetFull(int id, bool isSee = false)
        {
            if (isSee)
            {
                db.Update<ThreadEntity>("SET view_count=view_count+1 WHERE id=@0", id);
            }
            var model = db.SingleById<ThreadModel>(id); ;
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
            return model;
        }

        public bool Editable(ThreadEntity model)
        {
            if (environment.UserId == 0 || model.IsClosed > 0)
            {
                return false;
            }
            return model.UserId == environment.UserId || Can(model, "edit");
        }

        public ThreadPostEntity Reply(string content, int thread_id)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new Exception("请输入内容");
            }
            if (thread_id < 1)
            {
                throw new Exception("请选择帖子");
            }
            var thread = Get(thread_id);
            if (thread is null || thread.IsClosed > 0)
            {
                throw new Exception("帖子已关闭");
            }
            var max = db.FindScalar<int, ThreadPostEntity>("MAX(grade) as grade", "thread_id=@0", thread.Id);
            var post = new ThreadPostEntity() {
                Content = content,
                UserId = environment.UserId,
                ThreadId = thread_id,
                Grade = max + 1,
                Ip = environment.Ip,
            };
            if (!db.TrySave(post))
            {
                throw new Exception("发表失败");
            }
            new ForumRepository(db, userStore)
                .UpdateCount(thread.ForumId, "post_count");
            db.Update<ThreadEntity>("SET post_count=post_count+1, updated_at=@0 WHERE id=@1",
                environment.Now, thread_id);
            return post;
        }

        /**
         * 操作主题
         * @param int id
         * @return ThreadModel
         * @throws Exception
         */
        public ThreadEntity ThreadAction(int id, Dictionary<string, object> data)
        {
            var thread = Get(id);
            if (thread is null)
            {
                throw new Exception("请选择帖子");
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
            return thread;
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
            if (environment.UserId == 0)
            {
                return false;
            }
            return true;// TODO auth().User().HasRole("administrator");
        }

        public bool CanRemovePost(ThreadEntity model, ThreadPostEntity item)
        {
            if (environment.UserId == 0)
            {
                return false;
            }
            if (environment.UserId == model.UserId)
            {
                return true;
            }
            if (environment.UserId == item.UserId)
            {
                return true;
            }
            return true;//TODO auth().User().HasRole("administrator");
        }

        public void RemovePost(int id)
        {
            var item = db.SingleById<ThreadPostEntity>(id);
            if (item is null)
            {
                throw new Exception("请选择回帖");
            }
            var thread = Get(item.ThreadId);
            if (thread is null)
            {
                throw new Exception("请选择回帖");
            }
            if (!CanRemovePost(thread, item)) {
                throw new Exception("无权限");
            }
            db.Delete(item);
            new ForumRepository(db, userStore).UpdateCount(thread.ForumId, "post_count", -1);
            db.Update<ThreadEntity>("SET post_count=post_count-1, updated_at=@0 WHERE id=@1",
                environment.Now, thread.Id);
        }

        public void Remove(int id)
        {
            var thread = Get(id);
            if (thread is null || (thread.UserId != environment.UserId && Can(thread, "delete"))) {
                throw new Exception("操作失败");
            }
            db.Delete(thread);
            var count = db.FindCount<int, ThreadPostEntity>("COUNT(*) as c", "thread_id=@0", id) - 1;
            db.DeleteById<ThreadPostEntity>("thread_id", id);
            var repository = new ForumRepository(db, userStore);
            repository.UpdateCount(thread.ForumId, "thread_count", -1);
            repository.UpdateCount(thread.ForumId, "post_count", -count);
            db.Insert(new ForumLogEntity() { 
                ItemType = LogRepository.TYPE_THREAD,
                ItemId = id,
                Action = LogRepository.ACTION_DELETE,
                UserId = environment.UserId,
                CreatedAt = environment.Now,
            });
        }


        public IList<ThreadEntity> Suggestion(string keywords = "")
        {
            var sql = new Sql();
            sql.Select("id", "title").From<ThreadEntity>(db);
            SearchHelper.Where(sql, "title", keywords);
            sql.Limit(4);
            return db.Fetch<ThreadEntity>(sql);
        }

        public Page<ThreadLogModel> RewardList(int item_id, 
            byte item_type = 0, int page = 1)
        {
            var items = db.Page<ThreadLogModel>(page, 20,
                "WHERE item_type=@0 AND item_id=@1 AND action=@2 ORDER BY id DESC",
                item_type, item_id, LogRepository.ACTION_REWARD);
            userStore.WithUser(items.Items);
            return items;
        }

        private byte ToggleLike(ThreadEntity thread)
        {
            return new LogRepository(db).ToggleLog(environment.UserId,
                thread.Id,
                LogRepository.TYPE_THREAD,
                LogRepository.ACTION_AGREE, [
                LogRepository.ACTION_AGREE, LogRepository.ACTION_DISAGREE
            ]);
        }

        private void RewardThread(ThreadModel thread, float money)
        {
            if (thread.UserId == environment.UserId)
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

        public ThreadPostEntity? ChangePost(int id, byte status)
        {
            var model = db.SingleById<ThreadPostEntity>(id);
            if (model is null)
            {
                return null;
            }
            var thread = db.SingleById<ThreadEntity>(model.ThreadId);
            if (!Editable(thread)) {
                throw new Exception("无权限操作");
            }
            model.Status = status;
            db.TrySave(model);
            db.Insert(new ForumLogEntity()
            {
                ItemType = LogRepository.TYPE_POST,
                ItemId = id,
                Action = LogRepository.ACTION_STATUS,
                Data = status.ToString(),
                UserId = environment.UserId,
                CreatedAt = environment.Now,
            });
            return model;
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
