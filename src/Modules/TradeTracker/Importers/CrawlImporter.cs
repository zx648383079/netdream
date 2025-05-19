using NetDream.Modules.TradeTracker.Entities;
using NetDream.Modules.TradeTracker.Forms;
using NetDream.Modules.TradeTracker.Repositories;
using System.Linq;
using System.Text.Json.Nodes;

namespace NetDream.Modules.TradeTracker.Importers
{
    internal class CrawlImporter(TrackerContext db, int timestamp = 0)
    {

        private readonly TrackRepository _repository = new(db);

        public void Read(CrawlForm data)
        {
            foreach (var item in data.Items)
            {
                var channelId = GetChannelId(item.Channel);
                if (channelId < 1)
                {
                    continue;
                }
                var productId = FormatProductId(item.Product, channelId, data.Name ?? string.Empty);
                if (productId < 1)
                {
                    continue;
                }
                _repository.AddTradeLog(new TradeLogForm()
                {
                    ProductId = productId,
                    ChannelId = channelId,
                    Type = item.Type,
                    Price = item.Price,
                    OrderCount = item.OrderCount,
                    CreatedAt = item.CreatedAt == 0 ? timestamp : item.CreatedAt,
                });
            }
        }

        private int FormatProductId(JsonNode product, int channelId, string name = "")
        {
            if (product is null)
            {
                name = IDMapperImporter.FormatName(name);
                return db.Products.Where(i => i.Name == name || i.EnName == name).Select(i => i.Id).FirstOrDefault();
            }
            if (product.GetValueKind() == System.Text.Json.JsonValueKind.String)
            {
                return GetProductId(channelId, product.GetValue<string>());
            }
            var obj = product.AsObject();
            if (obj.TryGetPropertyValue("name", out var node))
            {
                name = IDMapperImporter.FormatName(node.GetValue<string>());
                return db.Products.Where(i => i.Name == name || i.EnName == name).Select(i => i.Id).FirstOrDefault();
            }
            if (obj.TryGetPropertyValue("channel", out var channel) && obj.TryGetPropertyValue("id", out var id))
            {
                return GetProductId(GetChannelId(channel.GetValue<string>()), id.GetValue<string>());
            }
            return 0;
        }


        private int GetProductId(int channelId, string val)
        {
            if (channelId < 1)
            {
                return 0;
            }
            return db.ChannelProducts.Where(i => i.ChannelId == channelId && i.PlatformNo == val).Select(i => i.ProductId).FirstOrDefault();
        }

        private int GetChannelId(string val)
        {
            var channelItems = GetChannelItems();
            foreach (var item in channelItems)
            {
                if (item.ShortName == val || item.SiteUrl.Contains(val))
                {
                    return item.Id;
                }
            }
            return 0;
        }

        private ChannelEntity[] GetChannelItems()
        {
            return db.Channels.Select(i => new ChannelEntity{
                Id = i.Id,
                ShortName = i.ShortName,
                SiteUrl = i.SiteUrl
            }).ToArray();
        }
    }
}
