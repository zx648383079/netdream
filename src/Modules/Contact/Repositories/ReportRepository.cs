using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Forms;
using NetDream.Modules.Contact.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NPoco;
using System;

namespace NetDream.Modules.Contact.Repositories
{
    public class ReportRepository(IDatabase db, IUserRepository userStore,
        IClientContext environment): ISystemFeedback
    {
        public Page<ReportModel> GetList(string keywords = "", 
            int itemType = 0, int itemId = 0, int type = 0, int page = 1)
        {
            var sql = new Sql();
            sql.Select("*").From<ReportEntity>(db);
            SearchHelper.Where(sql, ["title", "email", "content"], keywords);
            if (itemType > 0)
            {
                sql.Where("item_type=@0", itemType);
            }
            if (itemType > 0 && itemId > 0)
            {
                sql.Where("item_id=@0", itemId);
            }
            if (type > 0)
            {
                sql.Where("type=@0", type);
            }
            sql.OrderBy("status ASC", "id DESC");
            var items = db.Page<ReportModel>(page, 20, sql);
            userStore.WithUser(items.Items);
            return items;
        }

        public ReportEntity Get(int id)
        {
            return db.SingleById<ReportEntity>(id);
        }

        public ReportEntity Create(ReportForm data)
        {
            var model = new ReportEntity()
            {
                ItemType = data.ItemType,
                ItemId = data.ItemId,
                Type = data.Type,
                Title = data.Title,
                Content = data.Content,
                Email = data.Email,
                Files = data.Files,
                UserId = environment.UserId,
                Ip = environment.Ip,
            };
            if (!Check(model)) {
                throw new Exception("请不要重复操作");
            }
            db.TrySave(model);
            return model;
        }

        public bool Check(ReportEntity model)
        {
            var sql = new Sql();
            sql.Select("COUNT(*)")
                .From<ReportEntity>(db)
                .Where("item_id=@0 AND item_type=@1 AND status=0", model.ItemId, model.ItemType);
            if (model.UserId > 0)
            {
                sql.Where("user_id=@0", model.UserId);
            } else {
                sql.Where("ip=@0", model.Ip);
            }
            return db.ExecuteScalar<int>(sql) < 1;
        }
        public ReportEntity QuickCreate(byte itemType, int itemId,
            string content, string title)
        {
            return Create(new ReportForm()
            {
                ItemType = itemType,
                ItemId = itemId,
                Content = content,
                Type = 99,
                Title = title
            });
        }
        public ReportEntity QuickCreate(byte itemType, int itemId, 
            string content, byte type = 99)
        {
            return Create(new ReportForm()
            {
                ItemType = itemType,
                ItemId = itemId,
                Content = content,
                Type = type,
                Title = "其他投诉/举报"
            });
        }

        public int Report(byte itemType, int itemId, string content, string title)
        {
            var model = QuickCreate(itemType, itemId, content, title);
            return model.Id;
        }

        public ReportEntity Change(int id, int status)
        {
            var model = Get(id);
            model.Status = status;
            db.TrySave(model);
            return model;
        }

        public void Remove(params int[] id)
        {
            db.DeleteById<ReportEntity>(id);
        }
    }
}
