using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces.Entities;
using NPoco;

namespace NetDream.Shared.Repositories
{
    public abstract class CRUDRepository<T>(IDatabase db) where T : class
    {
        public virtual IList<string> SearchKeys => ["name"];

        public Page<T> GetList(string keywords = "", long page = 1)
        {
            var sql = new Sql();
            SearchHelper.Where(sql, SearchKeys, keywords);
            sql.OrderBy("created_at DESC");
            return db.Page<T>(page, 20, sql);
        }

        public T? Get(int id)
        {
            return db.SingleById<T>(id);
        }

        public T Save(int id, T entity) 
        {
            if (id == 0 && entity is IIdEntity o)
            {
                id = o.Id;
            }
            if (id > 0 && !db.Exists<T>(id))
            {
                throw new Exception("id is error");
            }
            var now = TimeHelper.TimestampNow();
            if (entity is ICreatedEntity c && c.CreatedAt == 0)
            {
                c.CreatedAt = now;
            }
            if (entity is ITimestampEntity t && t.UpdatedAt == 0)
            {
                t.UpdatedAt = now;
            }
            if (id > 0)
            {
                id = (int)db.Insert(entity);
                if (entity is IIdEntity i)
                {
                    i.Id = id;
                }
            } else
            {
                if (entity is IIdEntity i)
                {
                    i.Id = id;
                }
                db.Update(entity, id);
            }
            return entity;
        }

        public T Save(T entity)
        {
            return Save(0, entity);
        }

        protected virtual void AfterSave(int id, T data)
        {

        }

        public void Remove(int id)
        {
            if (db.Exists<T>(id))
            {
                return;
            }
            if (!RemoveWith(id))
            {
                return;
            }
            db.Delete<T>(id);
            UpdateCache(id);
        }

        protected virtual bool RemoveWith(int id)
        {
            return true;
        }

        protected virtual void UpdateCache(int id)
        {

        }
    }
}
