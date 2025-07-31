using NetDream.Modules.TradeTracker.Entities;
using NetDream.Shared.Helpers;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;

namespace NetDream.Modules.TradeTracker.Importers
{
    internal class IDMapperImporter(TrackerContext db)
    {
        public const string STEAM_NAME = "steam";

        private int GetChannelId(string name)
        {
            var id = db.Channels.Where(i => i.ShortName == name).Select(i => i.Id).FirstOrDefault();
            if (id > 0)
            {
                return id;
            }
            var model = new ChannelEntity()
            {
                Name = name,
                ShortName = name,
            };
            db.Channels.Save(model);
            db.SaveChanges();
            return model.Id;
        }

        public void Read(string fileName)
        {
            if (File.Exists(fileName))
            {
                ReadFromZip(fileName);
                return;
            }
            if (Directory.Exists(fileName))
            {
                ReadFromFolder(fileName);
                return;
            }
        }

        private int GetProductParent(int appId, string hashName, string name = "", string enName = "")
        {
            var id = db.Products.Where(i => i.UniqueCode == hashName).Select(i => i.Id).FirstOrDefault();
            if (id > 0)
            {
                return id;
            }
            var model = new ProductEntity()
            {
                Name = name,
                EnName = enName,
                ProjectId = appId,
                UniqueCode = hashName,
                IsSku = 0
            };
            db.Products.Add(model);
            db.SaveChanges();
            return model.Id;
        }

        private int GetProductId(int appId, string hashName, string name = "", string enName = "")
        {
            var(goodsHash, _) = SplitName(hashName);
            var product = db.Products.Where(i => i.UniqueCode == hashName).FirstOrDefault();
            if (product is not null)
            {
                return product.Id;
            }
            var parentId = 0;
            if (goodsHash != hashName)
            {
                var (goodsName, _) = SplitName(name);
                var (goodsEnName, _) = SplitName(enName);
                parentId = GetProductParent(appId, goodsHash, goodsName, goodsEnName);
            }
            var model = new ProductEntity()
            {
                Name = FormatName(name),
                EnName = FormatName(enName),
                ProjectId = appId,
                UniqueCode = hashName,
                ParentId = parentId,
                IsSku = 1
            };
            db.Products.Add(model);
            db.SaveChanges();
            return model.Id;
        }

        private void BindChannelProduct(int channelId, int productId, string hashName = "")
        {
            var exist = db.ChannelProducts.Where(i => i.ProductId == productId && i.ChannelId == channelId).Any();
            if (exist)
            {
                return;
            }
            db.ChannelProducts.Add(new()
            {
                ProductId = productId,
                ChannelId = channelId,
                PlatformNo = hashName
            });
            db.SaveChanges();
        }

        private void ReadFromZip(string file)
        {
            using var reader = ZipFile.OpenRead(file);
            ReadFromZip(reader);
        }

        private void ReadFromZip(ZipArchive reader)
        {
            var items = new Dictionary<string, Dictionary<int, ZipArchiveEntry>>();
            foreach (var item in reader.Entries)
            {
                if (!item.Name.EndsWith(".json"))
                {
                    continue;
                }
                var args = item.Name.Split('/');
                var appName = args[^2];
                var appId = int.Parse(args[^1][0..^5]);
                if (!items.ContainsKey(appName))
                {
                    items.Add(appName, []);
                }
                items[appName][appId] = item;
            }
            if (!items.TryGetValue(STEAM_NAME, out var target))
            {
                return;
            }
            foreach (var item in target)
            {
                using var doc = JsonDocument.Parse(item.Value.Open());
                ReadSteam(doc.RootElement, item.Key);
            }
            foreach (var item in items)
            {
                if (item.Key == STEAM_NAME)
                {
                    continue;
                }
                foreach (var it in item.Value)
                {
                    using var doc = JsonDocument.Parse(it.Value.Open());
                    ReadOther(doc.RootElement, item.Key, it.Key);
                }
            }
        }

        public void ReadFromZip(Stream input)
        {
            using var reader = new ZipArchive(input);
            ReadFromZip(reader);
        }

        private void ReadFromFolder(string folder)
        {
            var items = new Dictionary<string, Dictionary<int, string>>();
            foreach (var item in Directory.GetFiles("*.json", folder, SearchOption.AllDirectories))
            {
                var appName = Path.GetFileName(Path.GetDirectoryName(item));
                var appId = int.Parse(Path.GetFileNameWithoutExtension(item));
                if (!items.ContainsKey(appName))
                {
                    items.Add(appName, []);
                }
                items[appName][appId] = item;
            }
            if (!items.TryGetValue(STEAM_NAME, out var target))
            {
                return;
            }
            foreach (var item in target)
            {
                using var fs = File.OpenRead(item.Value);
                using var doc = JsonDocument.Parse(fs);
                ReadSteam(doc.RootElement, item.Key);
            }
            foreach (var item in items)
            {
                if (item.Key == STEAM_NAME)
                {
                    continue;
                }
                foreach (var it in item.Value)
                {
                    using var fs = File.OpenRead(it.Value);
                    using var doc = JsonDocument.Parse(fs);
                    ReadOther(doc.RootElement, item.Key, it.Key);
                }
            }
        }

        /**
         * @param int appId 730 cs; 570 dota
         * @return array{en_name: string, cn_name: string, name_id: string}[]
         */
        private void ReadSteam(JsonElement data, int appId = 730)
        {
            var channelId = GetChannelId(STEAM_NAME);
            var exist = db.Products.Where(i => i.ProjectId == appId)
                .Select(i => new KeyValuePair<string, int>(i.UniqueCode, i.Id)).ToDictionary();
            var add = new List<ProductEntity>();
            var now = TimeHelper.TimestampNow();
            foreach (var item in data.EnumerateObject())
            {
                var(goodsHash, _) = SplitName(item.Name);
                var cnName = item.Value.GetProperty("cn_name").GetString();
                var enName = item.Value.GetProperty("en_name").GetString();
                if (goodsHash != item.Name && !exist.ContainsKey(goodsHash))
                {
                    var(goodsName, _) = SplitName(cnName);
                    var(goodsEnName, _) = SplitName(enName);
                    var model = new ProductEntity()
                    {
                        Name = goodsName,
                        EnName = goodsEnName,
                        ProjectId = appId,
                        UniqueCode = goodsHash,
                        IsSku = 0,
                        UpdatedAt = now,
                        CreatedAt = now
                    };
                    db.Products.Add(model);
                    db.SaveChanges();
                    exist[goodsHash] = model.Id;
                }
                if (exist.ContainsKey(item.Name))
                {
                    continue;
                }
                add.Add(new()
                {
                    ParentId = goodsHash != item.Name ? exist[goodsHash] : 0,
                    Name = FormatName(cnName),
                    EnName = FormatName(enName),
                    ProjectId = appId,
                    UniqueCode = item.Name,
                    IsSku = 1,
                    UpdatedAt = now,
                    CreatedAt = now
                });
            }
            db.Products.AddRange(add);
            db.SaveChanges();
            var productIdItems = db.Products.Where(
                i => i.ProjectId == appId)
                .Select(i => new KeyValuePair<string, int>(i.UniqueCode, i.Id)).ToDictionary();
            var existLink = db.ChannelProducts.Where(i => i.ChannelId == channelId)
                .Select( i => i.ProductId).ToArray();
            foreach (var item in data.EnumerateObject())
            {
                var productId = productIdItems[item.Name];
                if (existLink.Contains(productId))
                {
                    continue;
                }
                db.ChannelProducts.Add(new ChannelProductEntity()
                {
                    ProductId = productId,
                    ChannelId = channelId,
                    PlatformNo = item.Value.GetProperty("name_id").GetString(),
                    ExtraMeta = item.Name,
                    UpdatedAt = now,
                    CreatedAt = now
                });
            }
            db.SaveChanges();
        }

        /**
         * 
         * @return array{en_name: string, name_id: string}[]
         */
        private void ReadOther(JsonElement data, string platformName, int appId = 730)
        {
            var channelId = GetChannelId(platformName);
            var productIdItems = db.Products.Where(i => i.ProjectId == appId)
                .Select(i => new KeyValuePair<string, int>(i.UniqueCode, i.Id)).ToDictionary();
            var exist = db.ChannelProducts.Where(i => i.ChannelId == channelId)
                .Select(i => i.ProductId).ToArray();
            var now = TimeHelper.TimestampNow();
            foreach (var item in data.EnumerateObject())
            {
                if (!productIdItems.TryGetValue(item.Name, out var productId))
                {
                    continue;
                }
                if (exist.Contains(productId))
                {
                    continue;
                }
                db.ChannelProducts.Add(new ChannelProductEntity()
                {
                    ProductId = productId,
                    ChannelId = channelId,
                    PlatformNo = item.Value.GetString(),
                    UpdatedAt = now,
                    CreatedAt = now
                });
            }
            db.SaveChanges();
        }

        public static string FormatName(string name)
        {
            return name.Replace('（', '(').Replace('）', ')');
        }

        public static (string, string) SplitName(string name) {
            name = FormatName(name);
            if (!name.EndsWith(')')) {
                return (name, string.Empty);
            }
            var i = name.IndexOf('(');
            if (i <= 0) {
                return (name, string.Empty);
            }
            return (name[..i].Trim(), name[(i + 1)..^2].Trim());
        }
    }
}
