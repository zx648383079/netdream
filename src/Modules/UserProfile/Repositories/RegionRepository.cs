using Microsoft.EntityFrameworkCore;
using NetDream.Modules.UserProfile.Entities;
using NetDream.Modules.UserProfile.Forms;
using NetDream.Modules.UserProfile.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace NetDream.Modules.UserProfile.Repositories
{
    public class RegionRepository(ProfileContext db, IClientContext client)
    {
        public IPage<RegionEntity> GetMangeList(QueryForm form, int parent = 0)
        {
            return db.Regions.Search(form.Keywords, "name")
                .Where(i => i.ParentId == parent)
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public RegionEntity? Get(int id)
        {
            return db.Regions.Where(i => i.Id == id).Single();
        }
        public IOperationResult<RegionEntity> Save(RegionForm data)
        {
            var model = data.Id > 0 ? db.Regions.Where(i => i.Id == data.Id)
                .Single() :
                new RegionEntity();
            if (model is null)
            {
                return OperationResult.Fail<RegionEntity>("id error");
            }
            model.Name = data.Name;
            db.Regions.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            var next = new int[] { id};
            while (next.Length > 0)
            {
                var items = db.Regions.Where(i => next.Contains(i.ParentId)).Pluck(i => i.Id);
                db.Regions.Where(i => next.Contains(i.Id)).ExecuteDelete();
                next = items;
            }
            db.SaveChanges();
        }

        /// <summary>
        /// 请导入以下文件
        /// @url https://github.com/modood/Administrative-divisions-of-China  dist/pcas.json
        /// </summary>
        /// <param name="input"></param>
        public void ImportFile(Stream input)
        {
            var doc = JsonDocument.Parse(input);
            if (doc is null)
            {
                return;
            }
            var root = doc.RootElement;
            var index = 0;
            foreach (var prov in root.EnumerateObject())
            {
                var provId = ++index;
                db.Regions.Add(new RegionEntity()
                {
                    Id = provId,
                    Name = prov.Name,
                    ParentId = 0,
                    FullName = prov.Name
                });
                foreach (var city in prov.Value.EnumerateObject())
                {
                    var cityId = ++index;
                    db.Regions.Add(new RegionEntity()
                    {
                        Id = cityId,
                        Name = city.Name,
                        ParentId = provId,
                        FullName = $"{prov.Name} {city.Name}"
                    });
                    foreach (var dist in city.Value.EnumerateObject())
                    {
                        var distId = ++index;
                        db.Regions.Add(new RegionEntity()
                        {
                            Id = distId,
                            Name = dist.Name,
                            ParentId = cityId,
                            FullName = $"{prov.Name} {city.Name} {dist.Name}"
                        });
                        foreach(var street in dist.Value.EnumerateArray())
                        {
                            var streetName = street.GetString();
                            if (string.IsNullOrWhiteSpace(streetName))
                            {
                                continue;
                            }
                            db.Regions.Add(new RegionEntity()
                            {
                                Id = ++index,
                                Name = streetName,
                                ParentId = distId,
                                FullName = $"{prov.Name} {city.Name} {dist.Name} {streetName}"
                            });
                        }
                    }
                }
            }
        }

        public static void Include(ProfileContext db, IWithRegionModel[] items)
        {
            var idItems = items.Select(item => item.RegionId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Regions.Where(i => idItems.Contains(i.Id))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.RegionId > 0 && data.TryGetValue(item.RegionId, out var res))
                {
                    item.Region = res;
                }
            }
        }
    }
}
