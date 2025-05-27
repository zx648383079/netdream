using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Modules.OnlineMedia.Forms;
using NetDream.Modules.OnlineMedia.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.OnlineMedia.Repositories
{
    public class MusicRepository(MediaContext db, IClientContext client)
    {
        public IPage<MusicEntity> GetList(QueryForm form)
        {
            return db.Music.Search(form.Keywords, "Name").ToPage(form);
        }

        public IOperationResult<MusicEntity> Get(int id)
        {
            var model = db.Music.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<MusicEntity>.Fail("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<MusicEntity> Save(MusicForm data)
        {
            var model = data.Id > 0 ? db.Music.Where(i => i.Id == data.Id)
                 .Single() :
                 new MusicEntity();
            if (model is null)
            {
                return OperationResult.Fail<MusicEntity>("id error");
            }
            model.Name = data.Name;
            db.Music.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Music.Where(i => i.Id == id).ExecuteDelete();
        }

        public MusicFileEntity[] FileList(int music)
        {
            return db.MusicFiles.Where(i => i.MusicId == music).ToArray();
        }

        public IOperationResult<MusicFileEntity> FileSave(MusicFileForm data)
        {
            var model = data.Id > 0 ? db.MusicFiles.Where(i => i.Id == data.Id)
                .SingleOrDefault() : new MusicFileEntity();
            if (model is null)
            {
                return OperationResult<MusicFileEntity>.Fail("数据错误");
            }
            model.MusicId = data.MusicId;
            model.FileType = data.FileType;
            model.File = data.File;
            db.MusicFiles.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void FileRemove(int id)
        {
            db.MusicFiles.Where(i => i.Id == id).ExecuteDelete();
        }

        public IPage<MusicModel> Search(QueryForm form, int[] idItems)
        {
            var res = db.Music.Search(form.Keywords, "name")
                .When(idItems.Length > 0, i => idItems.Contains(i.Id))
                .ToPage<MusicEntity, MusicModel>(form);
            IncludeFile(res.Items);
            return res;
        }

        private void IncludeFile(MusicModel[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            var idItems = items.Select(i => i.Id).ToArray();
            var data = db.MusicFiles.Where(i => idItems.Contains(i.MusicId))
                .ToArray();
            if (data.Length == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                item.Files = data.Where(i => i.MusicId == item.Id).ToArray();
            }
        }

        public ListLabelItem[] Suggestion(string keywords)
        {
            return db.Music.Search(keywords, "name").Take(4)
                .Select(i => new ListLabelItem(i.Id, i.Name))
                .ToArray();
        }

        public IOperationResult<string> Download(int id)
        {
            var model = db.MusicFiles.Where(i => i.Id == id).SingleOrDefault();
            if (model is null || model.FileType > 0)
            {
                return OperationResult<string>.Fail("文件不存在");
            }
            return OperationResult.Ok(model.File);
        }

        public IOperationResult<MusicModel> GetEdit(int id)
        {
            var model = db.Music.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<MusicModel>.Fail("数据有误");
            }
            var res = model.CopyTo<MusicModel>();
            res.Files = db.MusicFiles.Where(i => i.MusicId == model.Id).ToArray();
            return OperationResult.Ok(res);
        }

    }
}
