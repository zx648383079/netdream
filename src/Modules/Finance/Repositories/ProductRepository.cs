using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Finance.Entities;
using NetDream.Modules.Finance.Forms;
using NetDream.Modules.Finance.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Finance.Repositories
{
    public class ProductRepository(FinanceContext db, IClientContext client)
    {
        public FinancialProductEntity[] All()
        {
            return db.Product.Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.Id).ToArray();
        }

        /**
         * 获取
         * @param int id
         * @return FinancialProductModel
         * @throws Exception
         */
        public IOperationResult<FinancialProductEntity> Get(int id)
        {
            var model = db.Product.Where(i => i.UserId == client.UserId && i.Id == id)
                .FirstOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<FinancialProductEntity>("产品不存在");
            }
            return OperationResult.Ok(model);
        }

        /**
         * 保存
         * @param array data
         * @return FinancialProductModel
         * @throws Exception
         */
        public IOperationResult<FinancialProductEntity> Save(ProductForm data)
        {
            FinancialProductEntity? model;
            if (data.Id > 0)
            {
                model = db.Product.Where(i => i.UserId == client.UserId && i.Id == data.Id)
                .FirstOrDefault();
            }
            else
            {
                model = new();
            }
            if (model is null)
            {
                return OperationResult.Fail<FinancialProductEntity>("产品不存在");
            }
            model.Name = data.Name;
            model.UserId = client.UserId;
            model.Remark = data.Remark;
            db.Product.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /**
         * 删除产品
         * @param int id
         * @return mixed
         */
        public void Remove(int id)
        {
            db.Product.Where(i => i.UserId == client.UserId && i.Id == id)
                .ExecuteDelete();
            db.SaveChanges();
        }

        /**
         * 改变状态
         * @param int id
         * @return FinancialProductModel
         * @throws Exception
         */
        public IOperationResult<FinancialProductEntity> Change(int id)
        {
            var model = db.Product.Where(i => i.UserId == client.UserId && i.Id == id)
                .FirstOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<FinancialProductEntity>("产品不存在");
            }
            model.Status = (byte)(model.Status > 0 ? 0 : 1);
            db.Product.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public static void WithProduct(FinanceContext db, IWithProductModel[] items)
        {
            var idItems = items.Select(item => item.ProductId);
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Product.Where(i => idItems.Contains(i.Id))
                .Select(i => new ListLabelItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                }).ToArray();
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.ProductId == it.Id)
                    {
                        item.Product = it;
                        break;
                    }
                }
            }
        }
    }
}
