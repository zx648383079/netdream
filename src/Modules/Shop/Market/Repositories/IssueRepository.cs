using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class IssueRepository(ShopContext db, IClientContext client)
    {
        public IPage<GoodsIssueEntity> GetList(int itemId, QueryForm form)
        {
            return db.GoodsIssues.Where(i => i.GoodsId == itemId
                && i.Status != IssueStatus.STATUS_DELETE)
                .Search(form.Keywords, "question", "answer")
                .OrderByDescending(i => i.Status)
                .ToPage(form);
        }

        public IOperationResult Create(int item_id, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return OperationResult.Fail("question is empty");
            }
            var log = db.GoodsIssues.Where(i => i.GoodsId == item_id
                 && i.Question == content).FirstOrDefault();
            if (log is not null)
            {
                return OperationResult.Fail(string.Format("question is exist, {0}",
                    !string.IsNullOrWhiteSpace(log.Answer) ? "answer: " + log.Answer : "waiting for answer"));
            }
            db.GoodsIssues.Save(new GoodsIssueEntity()
            {
                AskId = client.UserId,
                GoodsId = item_id,
                Question = content,
            }, client.Now);
            return OperationResult.Ok();
        }

    }
}
