using Microsoft.EntityFrameworkCore;
using NetDream.Modules.UserProfile.Entities;
using NetDream.Modules.UserProfile.Forms;
using NetDream.Modules.UserProfile.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.UserProfile.Repositories
{
    public class BankCardRepository(ProfileContext db, IClientContext client)
    {
        public IPage<BankCardItem> GetList(QueryForm form)
        {
            var res = db.BankCards.Search(form.Keywords, "bank")
                .Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.Id)
                .ToPage<BankCardEntity, BankCardItem>(form);
            foreach (var item in res.Items)
            {
                item.Icon = "assets/images/wap_logo.png";
            }
            return res;
        }

        public IOperationResult<BankCardEntity> Get(int id)
        {
            var model = db.BankCards.Where(i => i.Id == id && i.UserId == client.UserId)
                .SingleOrDefault();
            if (model == null)
            {
                return OperationResult<BankCardEntity>.Fail("数据错误");
            }
            return OperationResult.Ok(model);
        }
        public IOperationResult<BankCardEntity> Save(BankCardForm data)
        {
            var model = data.Id > 0 ? db.BankCards
                .Where(i => i.Id == data.Id && i.UserId == client.UserId)
                .SingleOrDefault() :
                new BankCardEntity();
            if (model is null)
            {
                return OperationResult.Fail<BankCardEntity>("id error");
            }
            model.Bank = data.Bank;
            model.CardNo = data.CardNo;
            model.Type = data.Type;
            model.ExpiryDate = data.ExpiryDate;
            model.UserId = client.UserId;
            db.BankCards.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.BankCards.Where(i => i.Id == id && i.UserId == client.UserId)
                .ExecuteDelete();
            db.SaveChanges();
        }
    }
}
