using NetDream.Modules.TradeTracker.Entities;
using NetDream.Shared.Providers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace NetDream.Modules.TradeTracker.Importers
{
    /// <summary>
    /// https://github.com/EricZhu-42/SteamTradingSiteTracker-Data
    /// </summary>
    internal class DataImporter(TrackerContext db)
    {

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
                ReadFromJson(fileName);
                return;
            }
            if (Directory.Exists(fileName))
            {
                ReadFromFolder(fileName);
                return;
            }
        }

        private void ReadFromFolder(string folder)
        {
            foreach (var item in Directory.GetFiles("*.json", folder, SearchOption.AllDirectories))
            {
                ReadFromJson(item);
            }
        }

        public void ReadFromJson(Stream input)
        {
            var reader = new StreamReader(input);
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                using var doc = JsonDocument.Parse(line);
                UpdateLog(doc.RootElement);
            }
        }

        private void ReadFromJson(string file)
        {
            using var fs = File.OpenRead(file);
            ReadFromJson(fs);
        }

        private void UpdateLog(JsonElement data)
        {
            var (hashName, items, time) = FormatLog(data);
            var product = db.Products.Where(i => i.UniqueCode == hashName).FirstOrDefault();
            if (product is null)
            {
                return;
            }
            foreach (var item in items)
            {
                var channelId = GetChannelId(item.Key);
                foreach (var it in item.Value)
                {
                    if (it is null || (it.Price == 0 && it.OrderCount == 0))
                    {
                        continue;
                    }
                    db.Trades.Add(new TradeEntity()
                    {
                        ProductId = product.Id,
                        ChannelId = channelId,
                        Type = it.Type,
                        Price = it.Price,
                        OrderCount = it.OrderCount,
                        CreatedAt = time
                    });
                }
            }
        }


        private (string, Dictionary<string, TradeEntity[]>, int) FormatLog(JsonElement data)
        {
            var items = new Dictionary<string, TradeEntity[]>();
            var time = 0;
            if (data.TryGetProperty("created_at", out var createdAt))
            {
                time = createdAt.GetInt32();
                items[IDMapperImporter.STEAM_NAME] = [
                    new() { Price = data.GetProperty("optimal_sell_price").GetSingle() },
                    new() { Price = data.GetProperty("optimal_buy_price").GetSingle() }
                ];
                foreach (var item in data.EnumerateObject())
                {
                    var name = item.Name.Split('_', 2)[0];
                    if (!items.ContainsKey(name))
                    {
                        items[name] = new TradeEntity[2];
                    }
                    if (item.Name.EndsWith("_optimal_price"))
                    {
                        items[name][0] = new() { Price = item.Value.GetSingle() };
                    }
                    if (item.Name.EndsWith("_buy_num"))
                    {
                        // items[name][1].OrderCount = item.Value.GetInt32();
                    }
                    if (item.Name.EndsWith("_sell_num"))
                    {
                        items[name][0].OrderCount = item.Value.GetInt32();
                    }
                }
            }
            else
            {
                time = data.GetProperty("update_time").GetInt32();
                data.TryGetProperty("steam_order", out var node);
                items[IDMapperImporter.STEAM_NAME] = [
                    Parse(node, true),
                    Parse(node, false)
                ];
                foreach (var item in data.EnumerateObject())
                {
                    var name = item.Name.Split('_', 2)[0];
                    if (!items.ContainsKey(name))
                    {
                        items[name] = new TradeEntity[2];
                    }
                    if (item.Name.EndsWith("_buy"))
                    {
                        items[name][1] = Parse(item.Value, false);
                    }
                    if (item.Name.EndsWith("_sell"))
                    {
                        items[name][0] = Parse(item.Value, true);
                    }
                }
            }
            return (data.GetProperty("hash_name").GetString()!, items, time);
        }
        private static TradeEntity Parse(JsonElement data, bool isSell)
        {
            var res = new TradeEntity()
            {
                Type = (byte)(isSell ? 0 : 1)
            };
            if (data.TryGetProperty("price", out var node))
            {
                res.Price = node.GetSingle();
            }
            if (data.TryGetProperty(isSell ? "sell_price" : "buy_price", out node))
            {
                res.Price = node.GetSingle();
            }
            if (data.TryGetProperty("count", out node))
            {
                res.Price = node.GetInt32();
            }
            if (data.TryGetProperty(isSell ? "sell_order_count" : "buy_order_count", out node))
            {
                res.Price = node.GetInt32();
            }
            return res;
        }
    }
}
