using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Backend.Forms;
using NetDream.Modules.Shop.Backend.Models;
using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;
using System.Text.Json;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    public class DeliveryRepository(ShopContext db, IClientContext client,
        IUserRepository userStore)
    {
        public IPage<DeliveryListItem> GetList(QueryForm form)
        {
            var res = db.Deliveries.Search(form.Keywords, "name", "shipping_name", "logistics_number", "tel")
                .OrderBy(i => i.Status)
                .OrderByDescending(i => i.Id).ToPage<DeliveryEntity, DeliveryListItem>(form);
            userStore.Include(res.Items);
            OrderRepository.Include(db, res.Items);
            IncludeGoods(res.Items);
            return res;
        }

        private void IncludeGoods(DeliveryListItem[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            var idItems = items.Select(i => i.Id).ToArray();
            var data = db.DeliveryGoods.Where(i => idItems.Contains(i.DeliveryId))
                .ToArray();
            if (data.Length == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                item.Goods = data.Where(i => i.DeliveryId == item.Id).ToArray();
            }
        }

        public void Remove(int id)
        {
            db.Deliveries.Where(i => i.Id == id).ExecuteDelete();
            db.DeliveryGoods.Where(i => i.DeliveryId == id).ExecuteDelete();
            db.SaveChanges();
        }

        public IOperationResult<DeliveryEntity> Save(DeliveryForm data)
        {
            var model = db.Deliveries.Where(i => i.Id == data.Id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<DeliveryEntity>("数据有误");
            }
            if (data.LogisticsContent?.Length > 0)
            {
                model.LogisticsContent = JsonSerializer.Serialize(data.LogisticsContent);
                if (data.Status == 0)
                {
                    data.Status = data.LogisticsContent.Last().Status;
                }
            }
            if (data.Status > 0)
            {
                model.Status = data.Status;
            }
            db.Deliveries.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }
    }
}
