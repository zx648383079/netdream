using NetDream.Modules.TradeTracker.Entities;
using NetDream.Modules.TradeTracker.Models;
using System.Linq;

namespace NetDream.Modules.TradeTracker
{
    public static class Extension
    {


        internal static IQueryable<LogListItem> SelectAs(this IQueryable<TradeLogEntity> query)
        {
            return query.Select(i => new LogListItem()
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ChannelId = i.ChannelId,
                Type = i.Type,
                Price = i.Price,
                CreatedAt  = i.CreatedAt,
            });
        }

        internal static IQueryable<ProductListItem> SelectAs(this IQueryable<ProductEntity> query)
        {
            return query.Select(i => new ProductListItem()
            {
                Id = i.Id,
                Name = i.Name,
                EnName = i.EnName,
                ParentId = i.ParentId,
                ProjectId = i.ProjectId,
                CatId = i.CatId,
                UniqueCode = i.UniqueCode,
                UpdatedAt = i.UpdatedAt,
                IsSku = i.IsSku,
                CreatedAt = i.CreatedAt,
            });
        }
    }
}
