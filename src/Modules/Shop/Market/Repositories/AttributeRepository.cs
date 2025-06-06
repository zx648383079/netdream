using Microsoft.Extensions.Caching.Memory;
using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Models;
using NetDream.Modules.Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class AttributeRepository(ShopContext db, IMemoryCache cache)
    {
        /// <summary>
        /// 根据属性选择值获取货品和附加属性
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="goods"></param>
        /// <returns></returns>
        public AttributeResult GetProductAndPriceWithProperties(
            int[] properties, int goods) {
            return cache.GetOrCreate($"{goods}_{string.Join(',', properties)}", _ => {
                var res = SplitProperties(properties);
                if (res.ProductProperties?.Length > 0)
                {
                    res.Product = GetProduct(res.ProductProperties, goods);
                }
                return res;
            });
        }

        /// <summary>
        /// 根据属性值获取货品
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="goods"></param>
        /// <returns></returns>
        private ProductEntity? GetProduct(int[] properties, int goods) {
            var attributes = string.Join(GoodsStatus.ATTRIBUTE_LINK, properties.Order().ToArray());
            return db.Products.Where(i => i.Attributes == attributes
                && i.GoodsId == goods).FirstOrDefault();
        }
        public static int[] FormatPostProperties(string properties)
        {
            return FormatPostProperties(JsonSerializer.Deserialize<string[]>(properties));
        }
        public static int[] FormatPostProperties(string[] properties) 
        {
            if (properties is null || properties.Length == 0) {
                return [];
            }
            var data = new List<int>();
            foreach (var item in properties) {
                var args = item.Split(':');
                var value = int.Parse(args.Length > 1 ? args[1] : args[0]);
                if (value < 1 || data.Contains(value)) 
                {
                    continue;
                }
                data.Add(value);
            }
            return data.Order().ToArray();
        }

        /// <summary>
        /// 分离商品规格和附加属性
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public AttributeResult SplitProperties(int[] properties) 
        {
            if (properties is null || properties.Length == 0) 
            {
                return new();
            }
            var items = db.GoodsAttributes.Where(i => properties.Contains(i.Id))
                .Select(i => new GoodsAttributeEntity()
                {
                    AttributeId = i.AttributeId,
                    Id = i.Id,
                    Price = i.Price,
                    Value = i.Value
                })
                .ToArray();
            if (items.Length == 0)
            {
                return new();
            }

            var attrId = new List<int>();
            var data = new Dictionary<int, AttributeFormattedItem>();
            foreach (var item in items) 
            {
                attrId.Add(item.AttributeId);
                if (!data.TryGetValue(item.AttributeId, out var target)) 
                {
                    target = new AttributeFormattedItem();
                    data.Add(item.AttributeId, target);
                }
                target.Price += item.Price;
                target.Items.Add(item.Id);
                target.Label.Add(item.Value);
            }
            var propertiesPrice = 0f;
            var totalPropertiesPrice = 0f;
            var propertiesLabel = new List<string>();
            var attr_list = db.Attributes.Where(i => attrId.Contains(i.Id))
                .Select(i => new AttributeEntity()
                {
                    Id = i.Id,
                    Type = i.Type,
                    Name = i.Name
                }).ToArray();
            var formattedProperties = new List<int>();
            var productProperties = new List<int>();
            foreach (var item in attr_list) 
            {
                var group = data[item.Id];
                totalPropertiesPrice += group.Price;
                propertiesLabel.Add($"[{item.Name}]:{string.Join(',', group.Label)}");
                if (item.Type == 2) {
                    formattedProperties.AddRange(group.Items);
                    propertiesPrice += group.Price;
                    continue;
                }
                productProperties.AddRange(group.Items);
            }
            return new()
            {
                ProductProperties = productProperties.ToArray(),
                Properties = formattedProperties.ToArray(),
                PropertiesPrice = propertiesPrice,
                TotalPropertiesPrice = totalPropertiesPrice,
                PropertiesLabel = string.Join(';', propertiesLabel)
            };
        }

        public GoodsProperty[] GetProperties(int group, int goods) 
        {
            if (group < 1) 
            {
                return [];
            }
            var attr_list = db.Attributes.Where(i => i.GroupId == group && i.Type > 0)
                .OrderBy(i => i.Position)
                .Select(i => new AttributeEntity()
                {
                    Id = i.Id,
                    Name = i.Name,
                    PropertyGroup = i.PropertyGroup
                }).ToArray();
            if (attr_list.Length == 0)
            {
                return [];
            }
            var properties = new Dictionary<int, GoodsProperty>();
            var attrId = new List<int>();
            foreach (var item in attr_list)
            {
                attrId.Add(item.Id);
                properties[item.Id] = new GoodsProperty()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Type = item.Type,
                    AttrItems = []
                };
            }
            var items = db.GoodsAttributes.Where(i => i.GoodsId == goods
                && attrId.Contains(i.AttributeId)).ToArray();
            foreach (var item in items)
            {
                if (properties.TryGetValue(item.AttributeId, out var target))
                {
                    target.AttrItems.Add(item);
                    continue;
                }
            }
            return properties.Values.Where(i => i.AttrItems.Count > 0).ToArray();
        }

        public GoodsPropertyCollection[] GetStaticProperties(int group, int goods) {
            if (group < 1) {
                return [];
            }
            var attr_list = db.Attributes.Where(i => i.GroupId == group && i.Type == 0)
                .OrderBy(i => i.Position)
                .Select(i => new AttributeEntity()
                {
                    Id = i.Id,
                    Name = i.Name,
                    PropertyGroup = i.PropertyGroup,
                }).ToArray();
            if (attr_list.Length == 0) 
            {
                return [];
            }
            var static_properties = new Dictionary<string, GoodsPropertyCollection>();

            var attrId = new List<int>();
            foreach (var item in attr_list)
            {
                attrId.Add(item.Id);
                var groupName = item.PropertyGroup.Trim();
                if (!static_properties.TryGetValue(groupName, out var collection))
                {
                    collection = new GoodsPropertyCollection()
                    {
                        Name = groupName,
                        Items = []
                    };
                    static_properties.Add(groupName, collection);
                }
                if (collection.Items.Where(i => i.Id == item.Id).Any())
                {
                    continue;
                }
                collection.Items.Add(new()
                {
                    Name = item.Name,
                    Group = groupName,
                    Id = item.Id,
                });
            }
            var items = db.GoodsAttributes.Where(i => i.GoodsId == goods
                && attrId.Contains(i.AttributeId)).ToArray();
            foreach (var item in items)
            {
                foreach (var g in static_properties)
                {
                    foreach (var it in g.Value.Items)
                    {
                        if (it.Id == item.AttributeId)
                        {
                            it.AttrItem = item;
                            break;
                        }
                    }
                }
            }
            return static_properties.Values.Where(i => {
                i.Items = i.Items.Where(j => j.AttrItem is not null).ToArray();
                return i.Items.Count > 0;
            }).ToArray();
        }

        /// <summary>
        /// 一次性获取属性及静态属性
        /// </summary>
        /// <param name="group"></param>
        /// <param name="goods"></param>
        /// <returns></returns>
        public ProductPropertyResult BatchProperties(int group, int goods) {
            if (group < 1)
            {
                return new();
            }
            var attr_list = db.Attributes.Where(i => i.GroupId == group)
                .OrderBy(i => i.Position)
                .OrderBy(i => i.Type)
                .Select(i => new AttributeEntity()
                {
                    Id = i.Id,
                    Name = i.Name,
                    PropertyGroup = i.PropertyGroup,
                    Type = i.Type
                }).ToArray();
            if (attr_list.Length == 0) 
            {
                return new();
            }
            var properties = new Dictionary<int, GoodsProperty>();
            var static_properties = new Dictionary<string, GoodsPropertyCollection>();

            var attrId = new List<int>();
            foreach (var item in attr_list) 
            {
                attrId.Add(item.Id);
                if (item.Type > 0) {
                    properties[item.Id] = new GoodsProperty()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Type = item.Type,
                        AttrItems = []
                    };
                    continue;
                }
                var groupName = item.PropertyGroup.Trim();
                if (!static_properties.TryGetValue(groupName, out var collection))
                {
                    collection = new GoodsPropertyCollection()
                    {
                        Name = groupName,
                        Items = []
                    };
                    static_properties.Add(groupName, collection);
                }
                if (collection.Items.Where(i => i.Id == item.Id).Any())
                {
                    continue;
                }
                collection.Items.Add(new()
                {
                    Name = item.Name,
                    Group = groupName,
                    Id = item.Id,
                });
            }
            var items = db.GoodsAttributes.Where(i => i.GoodsId == goods
                && attrId.Contains(i.AttributeId)).ToArray();
            foreach (var item in items)
            {
                if (properties.TryGetValue(item.AttributeId, out var target)) {
                    target.AttrItems.Add(item);
                    continue;
                }
                foreach (var g in static_properties) 
                {
                    foreach (var it in g.Value.Items)
                    {
                        if (it.Id == item.AttributeId)
                        {
                            it.AttrItem = item;
                            break;
                        }
                    }
                }
            }
            return new ProductPropertyResult()
            {
                Properties = properties.Values.Where(i => i.AttrItems.Count > 0).ToArray(),
                StaticProperties = static_properties.Values.Where(i => {
                    i.Items = i.Items.Where(j => j.AttrItem is not null).ToArray();
                    return i.Items.Count > 0;
                }).ToArray()
            };
        }
    }
}
