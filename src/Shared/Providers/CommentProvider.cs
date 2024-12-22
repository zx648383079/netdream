using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Providers
{
    public class CommentProvider(ICommentContext db, IClientContext environment)
    {
        public const byte LOG_TYPE_COMMENT = 6;
        public const byte LOG_ACTION_AGREE = 1;
        public const byte LOG_ACTION_DISAGREE = 2;

        private ActionLogProvider _logger => new(db, environment);

        public IPage<CommentLog> Search(string keywords = "", int user = 0, 
            int target = 0, int parentId = -1, 
            string sort = "id", string order = "desc")
        {
            //list(sort, order) = SearchModel.CheckSortOrder(sort, order, [
            //    "id",
            //MigrationTable.COLUMN_CREATED_AT,
            //"agree_count"
            //]);
            //page = Query().When(user > 0, use(user)(object query) {
            //    query.Where("user_id", user);
            //})
            //.When(target > 0, use(target)(object query) {
            //    query.Where("target_id", target);
            //})
            //.When(parentId >= 0, use(parentId)(object query) {
            //    query.Where("parent_id", parentId);
            //})
            //.When(!empty(keywords), use(keywords)(object query) {
            //    SearchModel.SearchWhere(query, ["content"], false, string.Empty, keywords);
            //}).OrderBy(sort, order).Page();
            //data = page.GetPage();
            //if (empty(data))
            //{
            //    return page;
            //}
            //data = Relation.Create(FormatList(data), [
            //    "user" => Relation.Make(UserSimpleModel.Query(), "user_id", "id")
            //]);
            //page.SetPage(data);
            //return page;
            return null;
        }

        protected void FormatList(IList<CommentLog> data)
        {
            //idItems = array_column(data, "id");
            //count = Query().WhereIn("parent_id", idItems)
            //    .GroupBy("parent_id")
            //    .SelectRaw("COUNT(*) as count,parent_id as id").Get();
            //count = array_column(count, "count", "id");
            //foreach (data as k => item)
            //{
            //    val = Format(item);
            //    val["reply_count"] = isset(count[item["id"]]) ? intval(count[item["id"]]) : 0;
            //    item["agree_type"] = GetAgreeType(val["id"]);
            //    data[k] = val;
            //}
            //return data;
        }

        public void Remove(int id)
        {
            db.Comments.Where(i => i.Id == id).ExecuteDelete();
        }

        public int Insert(CommentEntity data)
        {
            //if (isset(data["extra_rule"]) && is_array(data["extra_rule"]))
            //{
            //    data["extra_rule"] = Json.Encode(data["extra_rule"]);
            //}
            db.Comments.Add(data);
            db.SaveChanges();
            if (data.Id == 0)
            {
                throw new Exception("insert log error");
            }
            return data.Id;
        }

        public CommentEntity? Get(int id)
        {
            return db.Comments.Where(i => i.Id == id).Single();
        }

        public CommentEntity Save(CommentEntity data)
        {
            data.UserId = environment.UserId;
            data.CreatedAt = environment.Now;
            db.Comments.Add(data);
            db.SaveChanges();
            // data["user"] = UserSimpleModel.ConverterFrom(auth().User());
            return data;
        }

        public void RemoveByTarget(int id)
        {
            db.Comments.Where(i => i.TargetId == id).ExecuteDelete();
        }

        public void RemoveBySelf(int id)
        {
            if (environment.UserId == 0) 
            {
                return;
            }
            db.Comments.Where(i => i.Id == id && i.UserId == environment.UserId).ExecuteDelete();
        }

        public CommentLog Agree(int id)
        {
            var model = Get(id);
            if (model is null)
            {
                throw new Exception("评论不存在");
            }
            var res = _logger.ToggleLog(LOG_TYPE_COMMENT,
                LOG_ACTION_AGREE, id,
                [LOG_ACTION_AGREE, LOG_ACTION_DISAGREE]);
            if (res < 1)
            {
                model.AgreeCount --;
            }
            else if(res == 1) {
                model.AgreeCount ++;
                model.DisagreeCount --;
            }
            else if(res == 2) {
                model.AgreeCount ++;
            }
            db.Comments.Update(model);
            db.SaveChanges();
            var log = new CommentLog(model);
            if (res > 0)
            {
                log.AgreeType = LOG_ACTION_AGREE;
            }
            return log;
        }

        public CommentLog Disagree(int id)
        {
            var model = Get(id);
            if (model is null)
            {
                throw new Exception("评论不存在");
            }
            var res = _logger.ToggleLog(LOG_TYPE_COMMENT,
                LOG_ACTION_DISAGREE, id,
                [LOG_ACTION_AGREE, LOG_ACTION_DISAGREE]);
            if (res < 1)
            {
                model.DisagreeCount--;
            }
            else if(res == 1) {
                model.AgreeCount--;
                model.DisagreeCount++;
            }
            else if(res == 2) {
                model.DisagreeCount++;
            }
            db.Comments.Update(model);
            db.SaveChanges();
            var log = new CommentLog(model);
            if (res > 0)
            {
                log.AgreeType = LOG_ACTION_DISAGREE;
            }
            return log;
        }

        /// <summary>
        /// 获取用户同意的类型
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public byte GetAgreeType(int comment)
        {
            if (environment.UserId == 0)
            {
                return 0;
            }
            var log = _logger.GetAction(LOG_TYPE_COMMENT, comment,
                [LOG_ACTION_AGREE, LOG_ACTION_DISAGREE]);
            return (byte)(log is null ? 0 : log);
        }
    }
}
