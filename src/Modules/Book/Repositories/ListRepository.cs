using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Forms;
using NetDream.Modules.Book.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Shared.Models;
using NPoco;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Book.Repositories
{
    public class ListRepository(IDatabase db, 
        IClientEnvironment environment,
        BookRepository bookStore,
        HistoryRepository historyStore,
        IUserRepository userStore)
    {
        public Page<ListEntity> GetList(string keywords = "", int page = 1)
        {
            var sql = new Sql();
            sql.Select().From<ListEntity>(db);
            SearchHelper.Where(sql, "title", keywords);
            sql.OrderBy("created_at desc");
            return db.Page<ListEntity>(page, 20, sql);
        }

        public ListModel Detail(int id)
        {
            var model = db.SingleById<ListModel>(id);
            if (model is null)
            {
                throw new Exception("书单不存在");
            }
            model.IsCollected = HasLog(BookRepository.LOG_TYPE_LIST,
                [BookRepository.LOG_ACTION_COLLECT], model.Id);
            var items = db.Fetch<ListItemModel>("WHERE list_id=@0", id);
            foreach (var item in items)
            {
                item.IsAgree = HasLog(BookRepository.LOG_TYPE_LIST, [BookRepository.LOG_ACTION_AGREE, BookRepository.LOG_ACTION_DISAGREE], id);
                item.OnShelf = historyStore.HasBook(item.BookId);
            }
            model.User = userStore.Get(model.UserId);
            model.Items = items;
            db.UpdateWhere<ListEntity>("click_count=click_count+1", "id=@0", id);
            return model;
        }

        private bool HasLog(byte type, byte[] action, int id)
        {
            var res = bookStore.Log().GetAction(type, id, action);
            return action.Length > 0 ? res > 0 : res is not null;
        }

        public ListEntity Save(ListForm data)
        {
            if (data.Items.Count == 0)
            {
                throw new Exception("请选择书籍");
            }
            var model = data.Id > 0 ? 
                db.SingleById<ListEntity>(data.Id) : new ListEntity() { 
                    UserId = environment.UserId,
                };
            if (model is null || model.UserId != environment.UserId)
            {
                throw new Exception("书单不存在");
            }
            model.Title = data.Title;
            model.Description = data.Description;
            if (!db.TrySave(model))
            {
                throw new Exception("error");
            }
            var exist = Array.Empty<int>();
            Sql sql;
            if (data.Id > 0)
            {
                sql = new Sql();
                sql.Select("id").From<ListItemEntity>(db)
                    .Where("list_id=@0", model.Id);
                exist = db.Pluck<int>(sql, "id").ToArray();
            }
            var add = new List<ListItemEntity>();
            var update = new List<int>();
            foreach (var item in data.Items)
            {
                if (item.BookId < 1)
                {
                    continue;
                }
                if (item.Id < 1 || !exist.Contains(item.Id))
                {
                    add.Add(new()
                    {
                        ListId = model.Id,
                        BookId = item.BookId,
                        Remark = item.Remark,
                        Star = item.Star < 0 ? 10 : item.Star
                    });
                    continue;
                }
                db.UpdateWhere<ListItemEntity>("book_id=@0, remark=@1, star=@2",
                    "list_id=@3 and id=@4", item.BookId, item.Remark, item.Star,
                    model.Id, item.Id);
                update.Add(item.Id);
            }
            var del = exist.Where(i => !update.Contains(i)).ToArray();
            if (del.Length > 0)
            {
                db.DeleteWhere<ListItemEntity>($"list_id={model.Id} and id in ({string.Join(',', del)})");
            }
            if (add.Count > 0)
            {
                db.InsertBatch(add);
            }
            model.BookCount = db.FindCount<ListItemEntity>("list_id=@0", model.Id);
            db.UpdateWhere<ListEntity>("book_count=@0", "id=@1", model.BookCount, model.Id);
            return model;
        }

        public void Remove(int id)
        {
            var model = db.SingleById<ListEntity>(id);
            if (model is null || model.UserId != environment.UserId)
            {
                throw new Exception("书单不存在");
            }
            db.DeleteById<ListEntity>(id);
            db.DeleteWhere<ListItemEntity>("list_id=@0", id);
        }

        public ListEntity Collect(int id)
        {
            var model = db.SingleById<ListModel>(id);
            if (model is null)
            {
                throw new Exception("书单不存在");
            }
            var res = bookStore.Log().ToggleLog(BookRepository.LOG_TYPE_LIST,
                BookRepository.LOG_ACTION_COLLECT, id);
            if (res > 0)
            {
                model.CollectCount++;
                model.IsCollected = true;
            }
            else
            {
                model.CollectCount--;
                model.IsCollected = false;
            }
            db.UpdateWhere<ListEntity>("collect_count=@0", "id=@1", model.CollectCount, id);
            return model;
        }

        public AgreeResult Agree(int id)
        {
            var model = db.SingleById<ListItemEntity>(id);
            if (model is null)
            {
                throw new Exception("书单不存在");
            }
            var res = bookStore.Log().ToggleLog(BookRepository.LOG_TYPE_LIST,
                BookRepository.LOG_ACTION_AGREE, id,
                [BookRepository.LOG_ACTION_AGREE, BookRepository.LOG_ACTION_DISAGREE]);
            var data = new AgreeResult()
            {
                AgreeCount = model.AgreeCount,
                DisagreeCount = model.DisagreeCount,
            };
            if (res < 1)
            {
                data.AgreeCount--;
                data.AgreeType = 0;
            }
            else if(res == 1) {
                data.AgreeCount++;
                data.DisagreeCount--;
                data.AgreeType = 1;
            }
            else if(res == 2) {
                data.AgreeCount++;
                data.AgreeType = 1;
            }
            db.UpdateWhere<ListItemEntity>("agree_count=@0, disagree_count=@1", "id=@2",
                data.AgreeCount, data.DisagreeCount, model.Id);
            return data;
        }

        public AgreeResult Disagree(int id)
        {
            var model = db.SingleById<ListItemEntity>(id);
            if (model is null)
            {
                throw new Exception("书单不存在");
            }
            var res = bookStore.Log().ToggleLog(BookRepository.LOG_TYPE_LIST,
                BookRepository.LOG_ACTION_DISAGREE, id,
                [BookRepository.LOG_ACTION_AGREE, BookRepository.LOG_ACTION_DISAGREE]);
            var data = new AgreeResult()
            {
                AgreeCount = model.AgreeCount,
                DisagreeCount = model.DisagreeCount,
            };
            if (res < 1)
            {
                data.DisagreeCount--;
                data.AgreeType = 0;
            }
            else if(res == 1) {
                data.AgreeCount--;
                data.DisagreeCount++;
                data.AgreeType = 2;
            }
            else if(res == 2) {
                data.DisagreeCount++;
                data.AgreeType = 2;
            }
            db.UpdateWhere<ListItemEntity>("agree_count=@0, disagree_count=@1", "id=@2",
                data.AgreeCount, data.DisagreeCount, model.Id);
            return data;
        }

    }
}
