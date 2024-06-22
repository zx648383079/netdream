using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces.Database;
using NetDream.Shared.Providers.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Providers
{
    public class TagProvider(IDatabase db, string prefix) : IMigrationProvider
    {
        private readonly string _mainTableName = prefix + "_tag";
        private readonly string _linkTableName = prefix + "_tag_link";
        public void Migration(IMigration migration)
        {
            migration.Append(_mainTableName, table => {
                table.Id();
                table.String("name", 20);
            }).Append(_linkTableName, table => {
                table.Uint("tag_id");
                table.Uint("target_id");
            });
        }

        public void Remove(int id) {
            db.Delete(_mainTableName, id);
            db.DeleteWhere(_linkTableName, "tag_id=@0", id);
        }

        /// <summary>
        /// 保存标签并获取标签的id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        public int[] Save(string[] name, IDictionary<string, object> append) {
            //var items = [];
            //idItems = [];
            //foreach ((array)name as item) {
            //    if (is_array(item)) {
            //        if (isset(item["id"]) && item["id"] > 0) {
            //            idItems[] = item["id"];
            //            continue;
            //        }
            //        item = item["name"] ?? null;
            //    }
            //    if (empty(item)) {
            //        continue;
            //    }
            //    if (in_array(name, items)) {
            //        continue;
            //    }
            //    items[] = item;
            //}
            //exist = TagQuery().WhereIn("name", items)
            //    .Pluck("id", "name");
            //foreach (items as name) {
            //    if (isset(exist[name])) {
            //        idItems[] = exist[name];
            //        continue;
            //    }
            //    id = TagQuery().Insert(array_merge(
            //        append,
            //        [
            //            "name" => name,
            //        ]
            //    ));
            //    if (id > 0) {
            //        idItems[] = id;
            //    }
            //}
            //return idItems;
            return [];
        }

        public Page<TagItem> GetList(string keywords = "", long page = 1, int perPage = 20) {
            var sql = new Sql();
            sql.Select("*")
                .From(_mainTableName);
            SearchHelper.Where(sql, ["name"], keywords);
            return db.Page<TagItem>(page, perPage, sql);
        }

        public IList<string> Suggest(string keywords = "", int count = 5) {
            var sql = new Sql();
            sql.Select("name").From(_mainTableName);
            SearchHelper.Where(sql, ["name"], keywords);
            sql.Limit(count);
            return db.Pluck<string>(sql);
        }

        public IList<TagItem> All() {
            var sql = new Sql();
            sql.Select("id", "name")
                .From(_mainTableName);
            return db.Fetch<TagItem>(sql);
        }

        public IList<TagItem> GetTags(int target) {
            var sql = new Sql();
            sql.Select("tag_id").From(_linkTableName)
                .Where("target_id=@0", target);
            var tagId = db.Pluck<int>(sql);
            if (tagId is null || tagId.Count == 0)
            {
                return [];
            }
            sql = new Sql();
            sql.Select("*")
                .From(_mainTableName).WhereIn("id", tagId.ToArray());
            return db.Fetch<TagItem>(sql);
        }

        public IDictionary<int, IList<TagItem>> GetManyTags(int[] target) {
            var sql = new Sql();
            sql.Select("tag_id", "target_id").From(_linkTableName)
                .WhereIn("target_id", target);
            var data = db.Fetch<TagLinkItem>(sql);
            var res = new Dictionary<int, IList<TagItem>>();
            if (data is null || data.Count == 0) {
                return res;
            }
            sql = new Sql();
            sql.Select("*").From(_mainTableName)
                .WhereIn("id", data.Select(i => i.TagId).Distinct().ToArray());
            var tags = db.Fetch<TagItem>(sql);
            foreach (var id in target) {
                var tagId = new List<int>();
                foreach (var item in data)
                {
                    if (item.TargetId == id)
                    {
                        tagId.Add(item.TagId);
                    }
                }
                if (tagId.Count == 0)
                {
                    continue;
                }
                var items = tags.Where(i => tagId.Contains(i.Id));
                if (items.Any())
                {
                    res.Add(id, items.ToArray());
                }
            }
            return res;
        }

        /// <summary>
        /// 根据关键词搜索标签表，并根据中间表找到链接的id集合
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public IList<int> SearchTag(string keywords = "") {
            var sql = new Sql();
            if (string.IsNullOrWhiteSpace(keywords)) {
                sql.Select("distinct target_id")
                    .From(_linkTableName);
                return db.Pluck<int>(sql);
            }
            sql.Select("id")
                .From(_mainTableName);
            SearchHelper.Where(sql, "name", keywords);
            var tagId = db.Pluck<int>(sql);
            if (tagId is null || tagId.Count == 0) {
                return [];
            }
            sql = new Sql();
            sql.Select("distinct target_id")
                    .From(_linkTableName).WhereIn("tag_id", tagId.ToArray());
            return db.Pluck<int>(sql);
        }

        /// <summary>
        /// 关联标签
        /// </summary>
        /// <param name="target">其他表的主键值</param>
        /// <param name="tags"></param>
        /// <param name="append"></param>
        /// <param name="afterBind"></param>
        public void BindTag(int target, string[] tags, IDictionary<string, object> append, Action<int[], int[], int[]>? afterBind = null) {
            var tagId = Save(tags, append);
            if (tagId.Length == 0) {
                return;
            }
            var (add, _, remove) = ModelHelper.SplitId(tagId,
                db.Pluck<int>(new Sql().Select("tag_id").From(_linkTableName)
                .Where("target_id=@0", target)));
            if (remove.Count > 0)
            {
                db.DeleteWhere(_linkTableName, 
                    $"target_id={target} AND tag_id IN ({string.Join(',', remove)})");
            }
            if (add.Count > 0)
            {
                db.InsertBatch(_linkTableName, add.Select(i => new Dictionary<string, object>()
                {
                    {"tag_id", i},
                    {"target_id", target}
                }));
            }
            afterBind?.Invoke(tagId, [.. add], [.. remove]);
        }


        public void RemoveLink(int target) {
            db.DeleteWhere(_linkTableName, "target_id=@0", target);
        }
    }
}
