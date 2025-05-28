using NetDream.Modules.Shop.Backend.Models;
using System;
using System.Linq;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    public class OrderRepository
    {
        internal static void Include(ShopContext db, IWithOrderModel[] items)
        {
            var idItems = items.Select(item => item.OrderId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Orders.Where(i => idItems.Contains(i.Id))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.OrderId > 0 && data.TryGetValue(item.OrderId, out var res))
                {
                    item.Order = res;
                }
            }
        }
    }
}