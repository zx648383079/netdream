using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Navigation.Entities;
using NetDream.Modules.Navigation.Forms;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Navigation.Repositories
{
    public class KeywordRepository(NavigationContext db)
    {

        public IOperationResult<KeywordEntity> Save(KeywordForm data)
        {
            if (db.Keywords.Where(i => i.Word == data.Word && i.Id != data.Id).Any())
            {
                return OperationResult.Fail<KeywordEntity>("关键词已存在");
            }
            var model = data.Id > 0 ? db.Keywords.Where(i => i.Id == data.Id)
                .SingleOrDefault() :
                new KeywordEntity();
            if (model is null)
            {
                return OperationResult.Fail<KeywordEntity>("id error");
            }
            model.Word = data.Word;
            model.Type = data.Type;
            db.Keywords.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }
        public int[] Save(string[] nameItems)
        {
            if (nameItems.Length == 0)
            {
                return [];
            }
            var exist = db.Keywords.Where(i => nameItems.Contains(i.Word)).ToArray();
            var items = nameItems.Select(i => new KeywordEntity() { Word = i }).ToArray();
            foreach (var item in items)
            {
                var it = Array.Find(exist, i => i.Word == item.Word);
                if (it is not null)
                {
                    item.Id = it.Id;
                    continue;
                }
                db.Keywords.Add(item);
            }
            db.SaveChanges();
            return items.Select(i => i.Id).ToArray();
        }

        public IPage<KeywordEntity> GetList(QueryForm form)
        {
            return db.Keywords.Search(form.Keywords, "word")
                .OrderBy(i => i.Id).ToPage(form);
        }

        public KeywordEntity[] AllList()
        {
            return db.Keywords.ToArray();
        }

        public int[] SearchTag(string keywords = "")
        {
            var tagId = db.Keywords.When(keywords, i => i.Word.Contains(keywords))
                .Select(i => i.Id).ToArray();
            if (tagId.Length == 0)
            {
                return [];
            }
            return db.PageKeywords.Where(i => tagId.Contains(i.WordId))
                .Select(i => i.PageId).Distinct().ToArray();
        }

        public KeywordEntity[] GetTags(int target)
        {
            var tagId = db.PageKeywords.Where(i => i.PageId == target).Select(i => i.WordId).ToArray();
            if (tagId is null || tagId.Length == 0)
            {
                return [];
            }
            return db.Keywords.Where(i => tagId.Contains(i.Id)).ToArray();
        }

        public void BindTag(int target, string[] tagItems)
        {
            var tagId = Save(tagItems);
            if (tagId.Length == 0)
            {
                return;
            }
            var (add, _, remove) = ModelHelper.SplitId(tagId,
                db.PageKeywords.Where(i => i.PageId == target).Select(i => i.WordId)
                .ToArray());
            if (remove.Count > 0)
            {
                db.PageKeywords.Where(i => i.PageId == target && remove.Contains(i.WordId)).ExecuteDelete();
            }
            if (add.Count > 0)
            {
                db.PageKeywords.AddRange(add.Select(i => new PageKeywordEntity()
                {
                    WordId = i,
                    PageId = target,
                }));
                db.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            db.Keywords.Where(i => i.Id == id).ExecuteDelete();
            db.PageKeywords.Where(i => i.WordId == id).ExecuteDelete();
            db.SaveChanges();
        }

        public void Remove(params string[] words)
        {
            if (words.Length == 0)
            {
                return;
            }
            var idItems = db.Keywords.Where(i => words.Contains(i.Word)).Pluck(i => i.Id);
            if (idItems.Length == 0)
            {
                return;
            }
            db.Keywords.Where(i => idItems.Contains(i.Id)).ExecuteDelete();
            db.PageKeywords.Where(i => idItems.Contains(i.WordId)).ExecuteDelete();
            db.SaveChanges();
        }
    }
}
