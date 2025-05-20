using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Finance.Entities;
using NetDream.Modules.Finance.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Finance.Repositories
{
    public class AccountRepository(FinanceContext db, IClientContext client)
    {
        public ListLabelItem[] GetItems()
        {
            return db.MoneyAccount.Where(i => i.UserId == client.UserId)
                .OrderBy(i => i.Id)
                .Select(i => new ListLabelItem(i.Id, i.Name)).ToArray();
        }

        /**
         * @return MoneyAccountModel[]
         */
        public MoneyAccountEntity[] All()
        {
            return db.MoneyAccount.Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.Id).ToArray();
        }

        /**
         * 获取
         * @param int id
         * @return MoneyAccountModel
         * @throws Exception
         */
        public IOperationResult<MoneyAccountEntity> Get(int id)
        {
            var model = db.MoneyAccount.Where(i => i.UserId == client.UserId && i.Id == id)
                .FirstOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<MoneyAccountEntity>("账户不存在");
            }
            return OperationResult.Ok(model);
        }

        /**
         * 保存
         * @param array data
         * @return MoneyAccountModel
         * @throws Exception
         */
        public IOperationResult<MoneyAccountEntity> Save(AccountForm data)
        {
            MoneyAccountEntity? model;
            if (data.Id > 0)
            {
                model = db.MoneyAccount.Where(i => i.UserId == client.UserId && i.Id == data.Id)
                .FirstOrDefault();
            }
            else
            {
                model = new();
            }
            if (model is null)
            {
                return OperationResult.Fail<MoneyAccountEntity>("账户不存在");
            }
            model.Name = data.Name;
            model.Remark = data.Remark;
            model.Money = data.Money;
            model.FrozenMoney = data.FrozenMoney;
            model.UserId = client.UserId;
            db.MoneyAccount.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /**
         * 软删除产品
         * @param int id
         * @return mixed
         */
        public void SoftDelete(int id)
        {
            db.MoneyAccount.Where(i => i.UserId == client.UserId && i.Id == id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.DeletedAt, client.Now));
            db.SaveChanges();
        }

        public void Remove(int id)
        {
            db.MoneyAccount.Where(i => i.UserId == client.UserId && i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        /**
         * 改变状态
         * @param int id
         * @return MoneyAccountModel
         * @throws Exception
         */
        public IOperationResult<MoneyAccountEntity> Change(int id)
        {
            var model = db.MoneyAccount.Where(i => i.UserId == client.UserId && i.Id == id)
                .FirstOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<MoneyAccountEntity>("账户不存在");
            }
            model.Status = (byte)(model.Status > 0 ? 0 : 1);
            db.MoneyAccount.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }
    }
}
