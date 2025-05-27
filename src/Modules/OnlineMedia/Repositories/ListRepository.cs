using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Modules.OnlineMedia.Forms;
using NetDream.Modules.OnlineMedia.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.OnlineMedia.Repositories
{
    public class ListRepository(MediaContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public IPage<MusicListItem> GetList(QueryForm form)
        {
            var res = db.MusicLists.Search(form.Keywords, "title")
                .OrderByDescending(i => i.CreatedAt)
                .ToPage<MusicListEntity, MusicListItem>(form);
            userStore.Include(res.Items);
            return res;
        }

        public IOperationResult<MusicListModel> Detail(int id)
        {
            var model = db.MusicLists.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<MusicListModel>.Fail("歌单不存在");
            }
            var idItems = db.MusicListItems.Where(i => i.ListId == id).Pluck(i => i.MusicId);
            var res = model.CopyTo<MusicListModel>();
            res.User = userStore.Get(model.UserId);
            res.Items = db.Music.Where(i => idItems.Contains(i.Id)).ToArray();
            return OperationResult.Ok(res);
        }



        public IOperationResult<MusicListEntity> Save(MusicListForm data, int[] items)
        {
            if (items.Length == 0)
            {
                return OperationResult<MusicListEntity>.Fail("请选择书籍");
            }
            var model = data.Id > 0 ? db.MusicLists.Where(i => i.UserId == client.UserId && i.Id == data.Id)
                .SingleOrDefault() : new MusicListEntity();
            if (model is null)
            {
                return OperationResult<MusicListEntity>.Fail("书单不存在");
            }
            model.Title = data.Title;
            model.Description = data.Description;
            model.Cover = data.Cover;
            model.UserId = client.UserId;
            db.MusicLists.Save(model, client.Now);
            db.SaveChanges();
            var exist = Array.Empty<int>();
            if (data.Id > 0)
            {
                exist = db.MusicListItems.Where(i => i.ListId == model.Id)
                    .Pluck(i => i.MusicId);
            }
            foreach (var item in items)
            {
                if (item < 1)
                {
                    continue;
                }
                if (exist.Contains(item))
                {
                    continue;
                }
                db.MusicListItems.Add(new MusicListItemEntity()
                {
                    ListId = model.Id,
                    MusicId = item
                });
            }
            var del = ModelHelper.Diff(exist, items);
            if (del.Length > 0)
            {
                db.MusicListItems.Where(i => i.ListId == model.Id
                    && del.Contains(i.MusicId))
                    .ExecuteDelete();
            }
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult Remove(int id)
        {
            var model = db.MusicLists.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("书单不存在");
            }
            db.MusicLists.Remove(model);
            db.MusicListItems.Where(i => i.ListId == model.Id).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

    }
}
