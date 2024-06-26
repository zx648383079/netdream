using NetDream.Modules.Contact.Entities;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NPoco;

namespace NetDream.Modules.Contact.Repositories
{
    public class SubscribeRepository(IDatabase db)
    {
        public Page<SubscribeEntity> GetList(string keywords = "", int page = 1)
        {
            var sql = new Sql();
            sql.Select("*").From<SubscribeEntity>(db);
            SearchHelper.Where(sql, ["email", "name"], keywords);
            sql.OrderBy("status ASC", "id DESC");
            return db.Page<SubscribeEntity>(page, 20, sql);
        }

        public void Change(int[] id, int status)
        {
            if (id.Length == 0)
            {
                return;
            }
            db.Update<SubscribeEntity>(new Sql().WhereIn("id", id), new Dictionary<string, object>()
            {
                {"status", status},
            });
        }

        public void Remove(params int[] id)
        {
            db.DeleteById<SubscribeEntity>(id);
        }
    }
}
