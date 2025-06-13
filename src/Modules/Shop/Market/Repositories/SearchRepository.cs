using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class SearchRepository(ShopContext db)
    {
        public string[] HotKeywords()
        {
            return db.Goods.Take(5).Pluck(i => i.Name);
        }

        public IFilterGroup[] FilterItems(string keywords, 
            int category = 0, int brand = 0)
        {
            var priceRange = db.Goods.Search(keywords, "name")
                .When(category > 0, i => i.CatId == category)
                .When(brand > 0, i => i.BrandId == brand)
                .GroupBy(i => i.Id)
                .Select(i => new {
                    Max = i.Max(j => j.Price),
                    Min = i.Min(j => j.Price)
                }).FirstOrDefault();
            var res = new List<IFilterGroup>()
            {
                RenderCategory(db.Categories.Where(i => i.ParentId == category).ToArray()),
                RenderBrand(db.Brands.ToArray()),
            };
            if (priceRange is not null)
            {
                res.Add(RenderPrice((float)priceRange.Min, (float)priceRange.Max));
            }
            return res.Where(i => i is not null).ToArray();
        }

        public IQueryable<GoodsEntity> FilterPrice(IQueryable<GoodsEntity> builder, 
            string price = "")
        {
            if (string.IsNullOrWhiteSpace(price))
            {
                return builder;
            }
            var args = price.Split('-').Select(i => decimal.Parse(i.Trim())).ToArray();
            if (args[0] > 0)
            {
                builder = builder.Where(i => i.Price >= args[0]);
            }
            if (args.Length > 1 && args[1] > args[0])
            {
                builder = builder.Where(i => i.Price <= args[1]);
            }
            return builder;
        }

        /**
         * 根据查询语句生成，数据多的化不建议使用，原理：通过遍历每一条数据进行统计
         * @param SqlBuilder builder
         * @return array
         */
        public IFilterGroup[] RenderQueryFilter(IQueryable<GoodsEntity> builder)
        {
            var catItems = new Dictionary<int, int>();
            var brandItems = new Dictionary<int, int>();
            var priceItems = new Dictionary<float, int>();
            var maxPrice = 0f;
            var minPrice = 0f;
            var query = builder.Select(i => new {
                i.CatId,
                i.BrandId,
                i.Price,
            });
            foreach (var item in query)
            {
                if (!catItems.TryAdd(item.CatId, 1))
                {
                    catItems[item.CatId]++;
                }
                if (brandItems.TryAdd(item.BrandId, 1))
                {
                    brandItems[item.CatId]++;
                }
                if (priceItems.TryAdd((float)item.Price, 1))
                {
                    priceItems[(float)item.Price]++;
                }
                if (minPrice <= 0 || (float)item.Price < minPrice)
                {
                    minPrice = (float)item.Price;
                }
                if (maxPrice <= 0 || (float)item.Price > maxPrice)
                {
                    maxPrice = (float)item.Price;
                }
            }
            var res = new List<IFilterGroup>()
            {
                RenderCategory(db.Categories.Where(i => catItems.Keys.Contains(i.Id))
                .ToArray(), catItems),
                RenderBrand(db.Brands.Where(i => brandItems.Keys.Contains(i.Id)).ToArray(), brandItems),
                RenderPrice(minPrice, maxPrice, priceItems)
            };
            return res.Where(i => i is not null).ToArray();
        }


        private IFilterGroup? RenderPrice(float min, float max, 
            Dictionary<float, int>? countItems = null)
        {
            if (min >= max)
            {
                return null;
            }
            var start = min = (float)Math.Floor(min);
            max = (float)Math.Ceiling(max);
            var maxCount = 10;
            var minStep = 30;
            var step = (float)Math.Max(Math.Ceiling((max - min) / maxCount), minStep);
            var data = new List<IFilterOption>() {
                new FilterOptionItem("不限", string.Empty)
                {
                    Count = countItems?.Values.Sum() ?? 0,
                }
            };
            while (start < max)
            {
                var next = start + step;
                var count = 0;
                if (countItems is not null)
                {
                    foreach (var item in countItems)
                    {
                        if (item.Key >= start && item.Key <= next)
                        {
                            count += item.Value;
                        }
                    }
                }
                var label = $"{start}-{next}";
                data.Add(new FilterOptionItem(label, label)
                {
                    Count = count
                });
                start = next;
            }
            return new RangeFilterGroup("price", "价格")
            {
                Min = min,
                Max = max,
                Items = data.ToArray()
            };
        }

        private IFilterGroup? RenderCategory(CategoryEntity[] items,
            Dictionary<int, int>? countItems = null)
        {
            if (items.Length == 0)
            {
                return null;
            }
            return new FilterGroup("category", "分类")
            {
                Items = items.Select(i => new FilterOptionItem(i.Name, i.Id.ToString())
                {
                    Count = countItems?.TryGetValue(i.Id, out var v) == true ? v : 0
                }).ToArray()
            };
        }

        private IFilterGroup? RenderBrand(BrandEntity[] items, 
            Dictionary<int, int>? countItems = null)
        {
            if (items.Length == 0)
            {
                return null;
            }
            return new FilterGroup("brand", "品牌")
            {
                Items = items.Select(i => new FilterOptionItem(i.Name, i.Id.ToString())
                {
                    Count = countItems?.TryGetValue(i.Id, out var v) == true ? v : 0
                }).ToArray()
            };
        }

    }
}
