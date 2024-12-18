using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Database;
using NetDream.Shared.Migrations;
using NetDream.Shared.Providers.Models;
using NPoco;
using System;
using System.Collections.Generic;

namespace NetDream.Shared.Providers
{
    public class CommentProvider(IDatabase db, string prefix, IClientContext environment) : IMigrationProvider
    {
        public const byte LOG_TYPE_COMMENT = 6;
        public const byte LOG_ACTION_AGREE = 1;
        public const byte LOG_ACTION_DISAGREE = 2;
        private readonly string _tableName = prefix + "_comment";

        private readonly ActionLogProvider _logger = new(db, prefix, environment);
        public void Migration(IMigration migration)
        {
            _logger.Migration(migration);
            migration.Append(_tableName, table => {
                table.Id();
                table.String("content");
                table.String("extra_rule", 300).Default(string.Empty)
                    .Comment("内容的一些附加规则");
                table.Uint("parent_id");
                table.Uint("user_id").Default(0);
                table.Uint("target_id");
                table.Uint("agree_count").Default(0);
                table.Uint("disagree_count").Default(0);
                table.Uint("status", 1).Default(0).Comment("审核状态");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
        }

        public Page<CommentLog> Search(string keywords = "", int user = 0, 
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
            db.Delete(_tableName, id);
        }

        public int Insert(CommentLog data)
        {
            //if (isset(data["extra_rule"]) && is_array(data["extra_rule"]))
            //{
            //    data["extra_rule"] = Json.Encode(data["extra_rule"]);
            //}
            data.Id = db.Insert(_tableName, data);
            if (data.Id == 0)
            {
                throw new Exception("insert log error");
            }
            return data.Id;
        }

        public void Update(int id, IDictionary<string, object> data)
        {
            db.UpdateById(_tableName, id, data);
        }


        public CommentLog? Get(int id)
        {
            return db.FindById<CommentLog>(_tableName, id);
        }

        public CommentLog Save(CommentLog data)
        {
            data.UserId = environment.UserId;
            data.CreatedAt = environment.Now;
            data.Id = db.Insert(_tableName, data);
            // data["user"] = UserSimpleModel.ConverterFrom(auth().User());
            return data;
        }

        public void RemoveByTarget(int id)
        {
            db.DeleteWhere(_tableName, "target_id=@0", id);
        }

        public void RemoveBySelf(int id)
        {
            if (environment.UserId == 0) 
            {
                return;
            }
            db.DeleteWhere(_tableName, "id=@0 AND user_id=@1", id, environment.UserId);
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
            Update(id, new Dictionary<string, object>()
            {
                { "agree_count", model.AgreeCount},
                { "disagree_count", model.DisagreeCount},
            });
            if (res > 0)
            {
                model.AgreeType = LOG_ACTION_AGREE;
            }
            return model;
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
            Update(id, new Dictionary<string, object>()
            {
                { "agree_count", model.AgreeCount},
                { "disagree_count", model.DisagreeCount},
            });
            if (res > 0)
            {
                model.AgreeType = LOG_ACTION_DISAGREE;
            }
            return model;
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
