using Microsoft.EntityFrameworkCore;
using NetDream.Modules.TradeTracker.Entities;
using NetDream.Modules.TradeTracker.Forms;
using NetDream.Modules.TradeTracker.Importers;
using NetDream.Modules.TradeTracker.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace NetDream.Modules.TradeTracker.Repositories
{
    public class ManagerRepository(TrackerContext db)
    {
        public IPage<ProductListItem> GetProductList(ProductQueryForm form)
        {
            return db.Products.Search(form.Keywords, "name", "en_name")
                .When(form.Category > 0, i => i.CatId == form.Category)
                .When(form.Project > 0, i => i.ProjectId == form.Project)
                .Where(i => i.IsSku == 1).ToPage(form, i => i.SelectAs());
        }
        public IOperationResult ProductRemove(int id)
        {
            var model = db.Products.Where(i => i.Id == id).FirstOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("id is error");
            }
            var items = new List<int>();
            if (model.ParentId < 1) {
                items = db.Products.Where(i => i.ParentId == id).Select(i => i.Id).ToList();
            }
            items.Add(id);
            db.ChannelProducts.Where(i => items.Contains(i.ProductId)).ExecuteDelete();
            db.Trades.Where(i => items.Contains(i.ProductId)).ExecuteDelete();
            db.Logs.Where(i => items.Contains(i.ProductId)).ExecuteDelete();
            db.Products.Where(i => items.Contains(i.Id)).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult<ProductEntity> ProductSave(ProductForm data) 
        {
            if (db.Products.Where(i => i.UniqueCode == data.UniqueCode && i.Id != data.Id).Any()) 
            {
                return OperationResult<ProductEntity>.Fail("已存在");
            }
            var model = data.Id > 0 ? db.Products.Where(i => i.Id == data.Id)
                .Single() :
                new ProductEntity();
            if (model is null)
            {
                return OperationResult.Fail<ProductEntity>("id error");
            }
            model.Name = data.Name;
            model.EnName = data.EnName;
            model.IsSku = data.IsSku;
            model.ParentId = data.ParentId;
            model.CatId = data.CatId;
            model.ProjectId = data.ProjectId;
            db.Products.Save(model);
            db.SaveChanges();
            if (data.Items is null || data.Items.Length == 0) 
            {
                return OperationResult.Ok(model);
            }
            foreach (var item in data.Items) 
            {
                var channelId = db.Channels.Where(i => i.ShortName == item.Channel)
                    .Select(i => i.Id).FirstOrDefault();
                if (channelId < 0)
                {
                    continue;
                }
                var exist = db.ChannelProducts.Where(i => i.ProductId == model.Id && i.ChannelId == channelId)
                    .Any();
                if (exist) {
                    continue;
                }
                db.ChannelProducts.Add( new ChannelProductEntity()
                {
                    ChannelId = channelId,
                    ProductId = model.Id,
                    PlatformNo = item.PlatformNo,
                    ExtraMeta = item.ExtraMeta,
                });
                db.SaveChanges();
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult ProductImport(string file) 
        {
            new IDMapperImporter(db).Read(file);
            return OperationResult.Ok();
        }

        public IOperationResult ProductImport(Stream input)
        {
            var importer = new IDMapperImporter(db);
            try
            {
                importer.ReadFromZip(input);
            }
            catch (Exception ex)
            {
                return OperationResult.Fail(ex.Message);
            }

            return OperationResult.Ok();
        }

        public IPage<ChannelEntity> ChannelList(QueryForm form) 
        {
            return db.Channels.Search(form.Keywords,  "name", "short_name")
                .OrderByDescending(i => i.Id).ToPage(form);
        }

        public IOperationResult<ChannelEntity> ChannelSave(ChannelForm data) 
        {
            if (db.Channels.Where(i => i.ShortName == data.ShortName && i.Id != data.Id).Any()) 
            {
                return OperationResult.Fail<ChannelEntity>("已存在");
            }
            var model = data.Id > 0 ? db.Channels.Where(i => i.Id == data.Id)
                .Single() :
                new ChannelEntity();
            if (model is null)
            {
                return OperationResult.Fail<ChannelEntity>("id error");
            }
            model.Name = data.Name;
            model.ShortName = data.ShortName;
            model.SiteUrl = data.SiteUrl;
            db.Channels.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void ChannelRemove(int id) {
            db.Channels.Where(i => i.Id == id).ExecuteDelete();
            db.ChannelProducts.Where(i => i.ChannelId == id).ExecuteDelete();
            db.SaveChanges();
        }

        public IPage<LogListItem> LogList(LogQueryForm form) 
        {
            var res = db.Logs.When(form.Product > 0, i => i.ProductId == form.Product)
                .When(form.Channel > 0, i => i.ChannelId == form.Channel)
                .When(form.Type > 0, i => i.Type == form.Type - 1)
                .OrderByDescending(i => i.CreatedAt).ToPage(form, i => i.SelectAs());
            ProductRepository.WithChannel(db, res.Items);
            ProductRepository.WithProduct(db, res.Items);
            return res;
        }

        public IOperationResult LogAdd(CrawlItemForm[] data) 
        {
            new CrawlImporter(db).Read(new CrawlForm()
            {
                Items = data
            });
            return OperationResult.Ok();
        }

        public void LogRemove(int[] id) {
            db.Logs.Where(i => id.Contains(i.Id)).ExecuteDelete();
            db.SaveChanges();
        }

        public void LogImport(string file) 
        {
            new DataImporter(db).Read(file);
        }

        public IOperationResult LogImport(Stream input)
        {
            var importer = new DataImporter(db);
            try
            {
                using var archive = new ZipArchive(input);
                foreach (var item in archive.Entries)
                {
                    if (!item.Name.EndsWith(".json"))
                    {
                        continue;
                    }
                    using var fs = item.Open();
                    importer.ReadFromJson(fs);
                }
            }
            catch (Exception ex)
            {
                return OperationResult.Fail(ex.Message);
            }
            
            return OperationResult.Ok();
        }

        public IOperationResult CrawlSave(CrawlForm data)
        {
            new CrawlImporter(db).Read(data);
            return OperationResult.Ok();
        }
    }
}
