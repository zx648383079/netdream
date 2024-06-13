using NetDream.Core.Extensions;
using NetDream.Core.Helpers;
using NPoco;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Core.Repositories
{
    public abstract class TagRepository<T>(IDatabase db) where T : class
    {
        protected virtual string IdKey => "id";
        protected virtual string NameKey => "name";
        public int[] Save(IList<string> nameItems)
        {
            return Save(nameItems, new Dictionary<string, object>());
        }
        public int[] Save(IList<string> nameItems, IDictionary<string, object> append)
        {
            // TODO
            return [];
        }

        public Page<T> GetList(string keywords = "", long page = 1)
        {
            var sql = new Sql();
            SearchHelper.Where(sql, [NameKey], keywords);
            return db.Page<T>(page, 20, sql);
        }

        public IList<T> AllList()
        {
            return db.Fetch<T>($"SELECT {IdKey},{NameKey}");
        }

        public IList<int> SearchTag<K>(string linkKey, string keywords = "", string tagKey = "tag_id")
            where K : class
        {
            var sql = new Sql();
            sql.From(ModelHelper.TableName(typeof(K)));
            sql.Select($"distinct {linkKey}");
            if (string.IsNullOrWhiteSpace(keywords))
            {
                return db.Pluck<int>(sql);
            }
            var tagSql = new Sql();
            tagSql.From(ModelHelper.TableName(typeof(T)));
            SearchHelper.Where(sql, NameKey, keywords);
            var tagId = db.Pluck<int>(tagSql);
            if (!tagId.Any())
            {
                return [];
            }
            sql.Where($"{tagKey} IN ({string.Join(',', tagId)})");
            return db.Pluck<int>(sql);
        }

        public void BindTag<K>(int linkId, string linkKey, 
            IList<string> tagItems, 
            IDictionary<string, object> append,
            string tagkey = "tag_id")
            where K : class
        {
            var tagId = Save(tagItems, append);
            if (tagId.Length == 0) 
            {
                return;
            }
            var (add, _, remove) = ModelHelper.SplitId(tagId,
                db.Pluck<int>(new Sql().From(ModelHelper.TableName(typeof(K)))
                .Where($"{linkKey}={linkId}"), tagkey));
            if (remove.Count > 0)
            {
                db.Delete<K>($"WHERE {linkKey}={linkId} AND {tagkey} IN ({string.Join(',', remove)})");
            }
            if (add.Count > 0) 
            {
                db.InsertBatch<K>(add.Select(i => new Dictionary<string, object>()
                {
                    {tagkey, i},
                    {linkKey, linkId }
                }));
            }
            OnAfterBind(tagId, add, remove);
        }

        protected virtual void OnAfterBind(int[] tagId, IList<int> add, IList<int> remove)
        {

        }
    }
}
