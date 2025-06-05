using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Backend.Forms;
using NetDream.Modules.Shop.Backend.Models;
using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    public class IssueRepository(ShopContext db, IClientContext client)
    {
        public IPage<IssueListItem> GetList(QueryForm form, int goods = 0)
        {
            var res = db.GoodsIssues
                .When(goods > 0, i => i.GoodsId == goods)
                .Search(form.Keywords, "question", "answer")
                .OrderBy(i => i.Status)
                .OrderByDescending(i => i.Id).ToPage<GoodsIssueEntity, IssueListItem>(form);
            GoodsRepository.Include(db, res.Items);
            return res;
        }

        public IOperationResult<GoodsIssueEntity> Save(IssueForm data)
        {
            var userId = client.UserId;
            var model = data.Id > 0 ? 
                db.GoodsIssues.Where(i => i.Id == data.Id).SingleOrDefault() 
                : new GoodsIssueEntity();
            if (model is null)
            {
                return OperationResult.Fail<GoodsIssueEntity>("数据错误");
            }
            model.GoodsId = data.GoodsId;
            model.Question = data.Question;
            model.Answer = data.Answer;
            model.Status = data.Status;
            if (model.AskId == 0)
            {
                model.AskId = userId;
            }
            if (model.AnswerId == 0)
            {
                model.AnswerId = userId;
            }
            db.GoodsIssues.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int[] idItems)
        {
            db.GoodsIssues.Where(i => idItems.Contains(i.Id))
                .ExecuteDelete();
            db.SaveChanges();
        }
    }
}
