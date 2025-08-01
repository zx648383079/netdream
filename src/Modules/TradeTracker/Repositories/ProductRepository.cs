﻿using NetDream.Modules.TradeTracker.Entities;
using NetDream.Modules.TradeTracker.Forms;
using NetDream.Modules.TradeTracker.Importers;
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
    public class ProductRepository(TrackerContext db)
    {
        public IPage<ProductListItem> GetGoodsList(ProductQueryForm form)
        {
            return db.Products.Search(form.Keywords, "name", "en_name")
                .When(form.Category > 0, i => i.CatId == form.Category)
                .When(form.Project > 0, i => i.ProjectId == form.Project)
                .Where(i => i.IsSku == 0).ToPage(form, i => i.SelectAs());
        }

        public IPage<ProductListItem> GetProductList(ProductQueryForm form)
        {
            return db.Products.Search(form.Keywords, "name", "en_name")
                .When(form.Category > 0, i => i.CatId == form.Category)
                .When(form.Project > 0, i => i.ProjectId == form.Project)
                .Where(i => i.IsSku == 1).ToPage(form, i => i.SelectAs());
        }

        public IOperationResult<ProductModel> Get(int id)
        {
            var model = db.Products.Where(i => i.Id == id).SingleOrDefault();
            if (model?.ParentId > 0)
            {
                model = db.Products.Where(i => i.Id == model.ParentId).SingleOrDefault();
            }
            if (model is null)
            {
                return OperationResult.Fail<ProductModel>("id is error");
            }
            var items = model.IsSku > 0 ? [] : db.Products.Where(i => i.ParentId == model.Id)
            .ToArray();
            var data = model.CopyTo<ProductModel>();
            data.Items = items.Select(i => {
                var (_, attr) = IDMapperImporter.SplitName(i.Name);
                return new ListLabelItem()
                {
                    Id = i.Id,
                    Name = attr,
                };
            }).ToArray();
            var channelId = db.ChannelProducts.Where(i => i.ProductId == id)
                .Select(i => i.ChannelId).ToArray();
            data.ChannelItems = channelId.Length == 0 ? [] : 
                db.Channels.Where(id => channelId.Contains(id.Id))
                .OrderBy(i => i.Id).ToArray();
            return OperationResult.Ok(data);
        }

        public TradeListItem[] GetPrice(int id)
        {
            var dayBegin = TimeHelper.TimestampFrom(DateTime.Today); ;
            var dayEnd = dayBegin + 86400;
            var items = db.Trades.Where(i => i.ProductId == id 
                && i.CreatedAt >= dayBegin && i.CreatedAt < dayEnd)
                .ToArray().CopyTo<TradeEntity, TradeListItem>();
            WithChannel(db, items);
            return items;
        }

        public TradeListItem[] GetPriceList(int id, int channel, int type = 0, 
            string startAt = "", string endAt = "")
        {
            var end = string.IsNullOrWhiteSpace(endAt) ? TimeHelper.TimestampNow() : TimeHelper.TimestampFrom(DateTime.Parse(endAt));
            var start = 0;
            if (string.IsNullOrWhiteSpace(startAt))
            {
                start = end - 31 * 86400;
            } else
            {
                start = TimeHelper.TimestampFrom(DateTime.Parse(startAt));
            }
            if (!endAt.Contains(' '))
            {
                end += 86400;
            }
            var items = db.Trades.Where(i => i.ProductId == id 
                && i.ChannelId == channel && i.Type == type)
            .When(start > 0, i => i.CreatedAt >= start)
            .When(!string.IsNullOrWhiteSpace(endAt), i => i.CreatedAt <= end)
            .OrderBy(i => i.CreatedAt)
            .ToArray();
            return items.CopyTo<TradeEntity, TradeListItem>();
        }

        public string[] Suggest(string keywords)
        {
            return db.Products.Search(keywords, "name", "en_name")
                .Where(i => i.ParentId == 0).Select(i => i.Name).ToArray();
        }

        internal static void WithProduct(TrackerContext db, IWithProductModel[] items)
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

        internal static void WithChannel(TrackerContext db, IWithChannelModel[] items)
        {
            var idItems = items.Select(item => item.ChannelId);
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Channels.Where(i => idItems.Contains(i.Id)).ToArray();
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.ChannelId == it.Id)
                    {
                        item.Channel = it;
                        break;
                    }
                }
            }
        }
    }
}
