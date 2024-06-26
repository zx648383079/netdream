using NetDream.Modules.Contact.Entities;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NPoco;
using NPoco.FluentMappings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Contact.Repositories
{
    public class FeedbackRepository(IDatabase db)
    {
        public Page<FeedbackEntity> ManageList(string keywords = "", int page = 1)
        {
            var sql = new Sql();
            sql.Select("*").From<FeedbackEntity>(db);
            SearchHelper.Where(sql, ["name", "email", "content"], keywords);
            sql.OrderBy("status ASC", "id DESC");
            return db.Page<FeedbackEntity>(page, 20, sql);
        }

        /// <summary>
        /// 允许前台显示的反馈内容
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="perPage"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public Page<FeedbackEntity> GetList(string keywords = "", 
            int perPage = 20, int page = 1)
        {
            var sql = new Sql();
            sql.Select("*").From<FeedbackEntity>(db);
            SearchHelper.Where(sql, ["name", "content"], keywords);
            sql.Where("open_status=1").OrderBy("id DESC");
            var items = db.Page<FeedbackEntity>(page, 20, sql);
            foreach (var item in items.Items)
            {
                item.Name = StrHelper.HideName(item.Name);
                item.Email = StrHelper.HideEmail(item.Email);
                item.Phone = StrHelper.HideTel(item.Phone);
            }
            return items;
        }

        public FeedbackEntity Get(int id)
        {
            return db.SingleById<FeedbackEntity>(id);
        }

        public FeedbackEntity? Change(int id, string[] data)
        {
            return ModelHelper.BatchToggle<FeedbackEntity>(db, id, data, ["status", "open_status"]);
        }

        public void Remove(params int[] id)
        {
            db.DeleteById<FeedbackEntity>(id);
        }
    }
}
