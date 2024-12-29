using Microsoft.EntityFrameworkCore;
using NetDream.Modules.SEO.Entities;
using NetDream.Modules.SEO.Forms;
using NetDream.Modules.SEO.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.SEO.Repositories
{
    public class OptionRepository(SEOContext db)
    {
        public GlobeOption LoadOption()
        {
            var data = new GlobeOption();
            var items = db.Options.Where(item => !string.IsNullOrEmpty(item.Code))
                .ToArray();
            foreach (var item in items)
            {
                var val = FormatOptionValue(item);
                if (val is null)
                {
                    continue;
                }
                data.Add(item.Code, val);
            }
            return data;
        }

        public static object? FormatOptionValue(OptionEntity option)
        {
            return option.Type switch
            {
                "switch" => option.Value == "1" || option.Value == "true",
                "json" => string.IsNullOrWhiteSpace(option.Value) ? null : "",
                _ => option.Value,
            };
        }

        public IList<OptionTreeModel> GetEditList()
        {
            /** @var OptionModel[]  items */
            var items = db.Options.Where(item => item.Visibility > 0)
                .OrderBy(item => item.ParentId).OrderByDescending(item => item.Position).ToArray();
            var res = items.Where(item => item.ParentId == 0).Select(i => new OptionTreeModel(i)).ToArray();
            foreach (var item in items)
            {
                if (item.ParentId == 0)
                {
                    continue;
                }
                foreach (var it in res)
                {
                    if (it.TryAdd(item))
                    {
                        break;
                    }
                }
            }
            return res;
        }

        public IDictionary<string, object> GetOpenList()
        {
            var data = new Dictionary<string, object>();
            var items = db.Options.Where(item => item.Visibility > 1)
                .OrderByDescending(item => item.Position).ToArray();
            foreach (var item in items)
            {
                var val = FormatOptionValue(item);
                if (val is null)
                {
                    continue;
                }
                data.TryAdd(item.Code, val);
            }
            return data;
        }

        public OptionEntity SaveNewOption(OptionForm data)
        {
            if (string.IsNullOrWhiteSpace(data.Name))
            {
                throw new Exception("名称不能为空");
            }
            if (db.Options.Where(item => item.Name == data.Name).Any())
            {
                throw new Exception("名称已存在");
            }
            OptionEntity entity;
            if (data.Type == "group")
            {
                entity = new OptionEntity()
                {
                    Name = data.Name,
                    Type = data.Type,
                };
            } else
            {
                if (string.IsNullOrWhiteSpace(data.Code) || data.ParentId < 1)
                {
                    throw new Exception("别名不能为空");
                }
                if (db.Options.Where(item => item.Code == data.Code).Any())
                {
                    throw new Exception("别名已存在");
                }
                entity = new OptionEntity()
                {
                    Name = data.Name,
                    Type = data.Type,
                    Code = data.Code,
                    Value = data.Value,
                    DefaultValue = data.DefaultValue,
                    Visibility = data.Visibility,
                    ParentId = data.ParentId,
                };
            }
            db.Options.Add(entity);
            db.SaveChanges();
            return entity;
        }

        public void SaveOption(IDictionary<int, string> data)
        {
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in data)
            {
                var entity = db.Options.Single(i => i.Id == item.Key);
                entity.Value = item.Value;
            }
            db.SaveChanges();
        }

        public OptionEntity Update(int id, OptionForm data)
        {
            var model = db.Options.Find(id);
            if (model is null || (model.Type == "group" && data.Type != model.Type))
            {
                throw new Exception("分组不能改成项");
            }
            if (db.Options.Where(i => i.Name == data.Name && i.Id != id).Any())
            {
                throw new Exception("名称重复");
            }
            if (model.Type == "group")
            {
                model.Name = data.Name;
            } else
            {
                if (string.IsNullOrWhiteSpace(data.Code) || data.ParentId < 1)
                {
                    throw new Exception("别名不能为空");
                }
                if (db.Options.Where(i => i.Code == data.Code && i.Id != id).Any())
                {
                    throw new Exception("别名已存在");
                }
                model.Name = data.Name;
                model.Type = data.Type;
                model.Code = data.Code;
                model.Value = data.Value;
                model.DefaultValue = data.DefaultValue;
                model.Visibility = data.Visibility;
                model.ParentId = data.ParentId;
            }
            db.SaveChanges();
            return model;
        }

        public void Remove(int id)
        {
            db.Options.Where(i => i.Id == id || i.ParentId == id).ExecuteDelete();
        }
    }
}
