using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Contact.Entities;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Contact.Repositories
{
    public class FeedbackRepository(ContactContext db)
    {
        public IPage<FeedbackEntity> ManageList(string keywords = "", int page = 1)
        {
            return db.Feedbacks.Search(keywords, "name", "email", "content")
                .OrderBy(i => i.Status)
                .OrderByDescending(i => i.Id).ToPage(page);
        }

        /// <summary>
        /// 允许前台显示的反馈内容
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="perPage"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IPage<FeedbackEntity> GetList(string keywords = "", 
            int perPage = 20, int page = 1)
        {
            var items = db.Feedbacks.Search(keywords, "name", "content")
                .Where(i => i.OpenStatus == 1).OrderByDescending(i => i.Id)
                .ToPage(page);
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
            return db.Feedbacks.Where(i => i.Id == id).Single();
        }

        public FeedbackEntity? Change(int id, string[] data)
        {
            var res = db.Feedbacks.BatchToggle(id, data, ["status", "open_status"]);
            if (res is not null)
            {
                db.SaveChanges();
            }
            return res;
        }

        public void Remove(params int[] id)
        {
            db.Feedbacks.Where(i => id.Contains(i.Id)).ExecuteDelete();
        }
    }
}
