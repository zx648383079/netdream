using Google.Protobuf.WellKnownTypes;
using NetDream.Modules.SEO.Entities;
using NetDream.Modules.SEO.Forms;
using NetDream.Modules.SEO.Models;
using NetDream.Shared.Extensions;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetDream.Modules.SEO.Repositories
{
    public class OptionRepository(IDatabase db)
    {
        public GlobeOption LoadOption()
        {
            var data = new GlobeOption();
            var items = db.Fetch<OptionEntity>();
            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.Code))
                {
                    continue;
                }
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
            var items = db.Fetch<OptionEntity>("WHERE visibility>0 ORDER BY parent_id ASC,position DESC");
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
            var data = new Dictionary<string, object?>();
            var items = db.Fetch<OptionEntity>("WHERE visibility>1 ORDER BY position DESC");
            foreach (var item in items)
            {
                data.TryAdd(item.Code, FormatOptionValue(item));
            }
            return data;
        }

        public OptionEntity SaveNewOption(OptionForm data)
        {
            if (string.IsNullOrWhiteSpace(data.Name))
            {
                throw new Exception("名称不能为空");
            }
            if (db.FindCount<OptionEntity>("name=@0", data.Name) > 0)
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
                if (db.FindCount<OptionEntity>("code=@0", data.Code) > 0)
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
            entity.Id = (int)db.Insert(entity);
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
                db.Update<OptionEntity>("SET value=@0 WHERE id=@1", item.Value, item.Key);
            }
        }

        public OptionEntity Update(int id, OptionForm data)
        {
            var model = db.SingleById<OptionEntity>(id);
            if (model.Type == "group" && data.Type != model.Type)
            {
                throw new Exception("分组不能改成项");
            }
            if (db.FindCount<OptionEntity>("name=@0 AND id<>@1", data.Name, id) > 0)
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
                if (db.FindCount<OptionEntity>("code=@0 AND id<>@1", data.Code, id) > 0)
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
            db.Save(model);
            return model;
        }

        public void Remove(int id)
        {
            db.Delete<OptionEntity>(id);
            db.Delete<OptionEntity>("WHERE parent_id=@0", id);
        }
    }
}
