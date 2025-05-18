using Microsoft.EntityFrameworkCore;
using NetDream.Modules.TradeTracker.Entities;
using NetDream.Modules.TradeTracker.Forms;
using NetDream.Modules.TradeTracker.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.TradeTracker.Repositories
{
    public class TrackRepository(TrackerContext db)
    {
        public IPage<TradeListItem> LatestList(QueryForm form,
            int channel = 0, int project = 0, int product = 0)
        {
            var dayBegin = TimeHelper.TimestampFrom(DateTime.Today); ;
            var dayEnd = dayBegin + 86400;
            // select a.* from tb a inner join (select name , min(val) val from tb group by name) b on a.name = b.name and a.val = b.val order by a.name
            var productId = Array.Empty<int>();
            var isFilter = false;
            if (product > 0)
            {
                isFilter = true;
                productId = db.Products.Where(i => i.Id == product || i.ParentId == product)
                    .Where(i => i.IsSku == 1).Select(i => i.Id).ToArray();
            }
            else if (!string.IsNullOrWhiteSpace(form.Keywords) || project > 0)
            {
                isFilter = true;
                productId = db.Products.Search(form.Keywords, "name", "en_name")
                .When(project > 0, i => i.ProjectId == project)
               .Where(i => i.IsSku == 1).Select(i => i.Id).ToArray();
            }
            if (isFilter && productId.Length == 0)
            {
                return new Page<TradeListItem>();
            }
            var total = db.Trades
                .When(channel > 0, i => i.ChannelId == channel)
                .When(productId.Length > 0, i => productId.Contains(i.ProductId))
                .Where(i => i.CreatedAt >= dayBegin && i.CreatedAt < dayEnd)
                .GroupBy(i => i.ProductId).Count();
            var res = new Page<TradeListItem>(total, form);
            if (res.IsEmpty)
            {
                return res;
            }
            res.Items = db.Trades
                .When(channel > 0, i => i.ChannelId == channel)
                .When(productId.Length > 0, i => productId.Contains(i.ProductId))
                .Where(i => i.CreatedAt >= dayBegin && i.CreatedAt < dayEnd)
                .Join(
                    db.Trades.When(channel > 0, i => i.ChannelId == channel)
                    .When(productId.Length > 0, i => productId.Contains(i.ProductId))
                    .Where(i => i.CreatedAt >= dayBegin && i.CreatedAt < dayEnd)
                    .GroupBy(i => i.ProductId)
                    .Select(i => new { ProductId = i.Key, Price = i.Min(i => i.Price) }),
                    a => new { a.ProductId, a.Price },
                    b => new { b.ProductId, b.Price },
                    (a, b) => a
                ).OrderByDescending(i => i.CreatedAt).Skip(res.ItemsOffset)
                .Take(res.ItemsPerPage).ToArray().CopyTo<TradeEntity, TradeListItem>();
            ProductRepository.WithChannel(db, res.Items);
            WithProduct(res.Items);
            return res;
        }

        private void WithProduct(IWithProductModel[] items)
        {
            var idItems = items.Select(item => item.ProductId);
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Products.Where(i => idItems.Contains(i.Id))
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

        



        /**
         * 
         * @param array{product_id: int, channel_id: int, type?: int, price: float, order_count?: int, created_at: int|string} data
         */
        public void AddTradeLog(TradeLogForm data)
        {
            if (data.Price <= 0 || data.Price >= 9999999)
            {
                if (data.OrderCount > 0)
                {
                    UpdateTrade(data);
                }
                return;
            }

            AddLog(new TradeLogEntity()
            {
                ProductId = data.ProductId,
                ChannelId = data.ChannelId,
                Type = data.Type,
                Price = data.Price,
                CreatedAt = data.CreatedAt,
            });
            AddTrade(new TradeEntity()
            {
                ProductId = data.ProductId,
                ChannelId = data.ChannelId,
                Type = data.Type,
                Price = data.Price,
                CreatedAt = data.CreatedAt,
                OrderCount = data.OrderCount,
            });
        }

        private void UpdateTrade(TradeLogForm data)
        {
            var dayBeign = TimeHelper.TimestampFrom(TimeHelper.TimestampTo(data.CreatedAt).Date);
            var dayEnd = dayBeign + 86400;
            db.Trades.Where(i => i.ProductId == data.ProductId
                && i.ChannelId == data.ChannelId && i.Type == data.Type
                && i.CreatedAt >= dayBeign && i.CreatedAt < dayEnd
                && i.OrderCount < data.OrderCount)
                .ExecuteUpdate(setter => setter.SetProperty(i => i.OrderCount, data.OrderCount));
            db.SaveChanges();
        }

        private void AddTrade(TradeEntity data)
        {
            var dayBeign = TimeHelper.TimestampFrom(TimeHelper.TimestampTo(data.CreatedAt).Date);
            var dayEnd = dayBeign + 86400;
            var log = db.Trades.Where(i => i.ProductId == data.ProductId
                && i.ChannelId == data.ChannelId && i.Type == data.Type
                && i.CreatedAt >= dayBeign && i.CreatedAt < dayEnd)
                .FirstOrDefault();
            if (log is null)
            {
                db.Trades.Save(data);
                db.SaveChanges();
                return;
            }
            if (log.CreatedAt == data.CreatedAt)
            {
                return;
            }
            if (log.OrderCount < data.OrderCount)
            {
                log.OrderCount = data.OrderCount;
            }
            if ((data.Type > 0 && log.Price < data.Price) ||
            (data.Type < 1 && log.Price > data.Price))
            {
                log.Price = data.Price;
            }
            db.Trades.Save(log);
            db.SaveChanges();
        }

        private void AddLog(TradeLogEntity data)
        {
            var exist = db.Logs.Where(i => i.ProductId == data.ProductId
                && i.ChannelId == data.ChannelId && i.Type == data.Type
                && i.CreatedAt <= data.CreatedAt && i.CreatedAt > data.CreatedAt - 3600)
                .OrderByDescending(i => i.CreatedAt).FirstOrDefault();
            if (exist is not null && (exist.Price == data.Price || exist.CreatedAt == data.CreatedAt))
            {
                return;
            }
            db.Logs.Save(data);
            db.SaveChanges();
        }
        public PriceListItem[] BatchLatestList(string channel, string product, string to)
        {
            return BatchLatestList(channel, [product], to);
        }
        public PriceListItem[] BatchLatestList(string channel, string[] product, string to)
        {
            var channelId = string.IsNullOrWhiteSpace(channel) ? 0 :
                db.Channels.Where(i => i.ShortName == channel).Select(i => i.Id).FirstOrDefault();
            var maps = new Dictionary<int, string>();
            int[] productId;
            if (channelId < 1)
            {
                productId = product.Select(int.Parse).Where(i => i > 0).ToArray();
            }
            else
            {
                product = product.Where(i => !string.IsNullOrWhiteSpace(i)).ToArray();
                if (product.Length == 0)
                {
                    return [];
                }
                maps = db.ChannelProducts
                    .Where(i => i.ChannelId == channelId && product.Contains(i.PlatformNo))
                    .ToDictionary(i => i.ProductId, i => i.PlatformNo);
                productId = maps.Keys.ToArray();
            }
            if (productId.Length == 0)
            {
                return [];
            }
            var toChannelId = to == channel ? channelId : db.Channels.Where(i => i.ShortName == to).Select(i => i.Id).FirstOrDefault();
            if (toChannelId < 1)
            {
                return [];
            }
            var dayBegin = TimeHelper.TimestampFrom(DateTime.Today);
            var dayEnd = dayBegin + 86400;
            var items = db.Trades
                .Where(i => productId.Contains(i.ProductId)
                && i.CreatedAt >= dayBegin && i.CreatedAt < dayEnd
                && i.ChannelId == toChannelId && i.Type == 0)
                .Select(i => new {
                    i.ProductId,
                    i.CreatedAt,
                    i.Price
                }).ToArray();
            return items.Select(i => new PriceListItem()
            {
                Product = maps.Count == 0 ? i.ProductId.ToString() : maps[i.ProductId],
                Price = i.Price,
                CreatedAt = i.CreatedAt
            }).ToArray();
        }
    }
}
