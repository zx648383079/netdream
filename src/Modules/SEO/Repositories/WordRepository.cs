using Microsoft.EntityFrameworkCore;
using NetDream.Modules.SEO.Entities;
using NetDream.Modules.SEO.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.SEO.Repositories
{
    public class WordRepository(SEOContext db)
    {
        public IPage<BlackWordEntity> GetList(QueryForm form)
        {
            return db.BlackWords.Search(form.Keywords, "words", "replace_words")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public BlackWordEntity? Get(int id)
        {
            return db.BlackWords.Where(i => i.Id == id).Single();
        }
        public IOperationResult<BlackWordEntity> Save(WordForm data)
        {
            var model = data.Id > 0 ? db.BlackWords.Where(i => i.Id == data.Id).Single() :
                new BlackWordEntity();
            if (model is null)
            {
                return OperationResult.Fail<BlackWordEntity>("id error");
            }
            model.Words = data.Words;
            model.ReplaceWords = data.ReplaceWords;
            db.BlackWords.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.BlackWords.Where(i => i.Id == id).ExecuteDelete();
        }
    }
}
