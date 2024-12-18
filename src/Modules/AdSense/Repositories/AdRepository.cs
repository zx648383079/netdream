using Modules.AdSense.Entities;
using NetDream.Modules.AdSense.Forms;
using NetDream.Modules.AdSense.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NPoco;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.AdSense.Repositories
{
    public class AdRepository(IDatabase db, IClientContext environment)
    {
        public const int TYPE_TEXT = 0;
        public const int TYPE_IMAGE = 1;
        public const int TYPE_HTML = 2;
        public const int TYPE_VIDEO = 3;

        public void ManagePositionRemove(int id)
        {
            db.Delete<PositionEntity>(id);
            db.Delete<AdEntity>("WHERE position_id=@0", id);
        }

        public IList<PositionEntity> PositionAll()
        {
            return db.Fetch<PositionEntity>();
        }

        public IList<FormattedAdModel> Banners()
        {
            return Load("banner");
        }

        public IList<FormattedAdModel> MobileBanners()
        {
            return Load("mobile_banner");
        }
        public IList<AdModel> GetList(string keywords, string position)
        {
            var id = 0;
            if (string.IsNullOrWhiteSpace(position))
            {
                id = db.FindScalar<int, PositionEntity>("id", "name=@0", position);
                if (id == 0)
                {
                    return [];
                }
            }
            return GetList(keywords, id);
        }
        public IList<AdModel> GetList(string keywords = "", int position = 0)
        {
            var sql = new Sql();
            SearchHelper.Where(sql, "name", keywords);
            if (position > 0)
            {
                sql.Where("position_id=@0", position);
            }
            sql.Where("start_at<=@0 AND end_at>@0", environment.Now);
            var items = db.Fetch<AdModel>(sql);
            WithPosition(items);
            return items;
        }

        private void WithPosition(IEnumerable<AdModel>? items)
        {
            if (items is null)
            {
                return;
            }
            var idItems = items.Select(item => item.PositionId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Fetch<PositionEntity>($"WHERE id IN ({string.Join(',', idItems)})");
            if (data is null || data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.PositionId == it.Id)
                    {
                        item.Position = it;
                        break;
                    }
                }
            }
        }
        private void WithPosition(AdModel? item)
        {
            if (item is null)
            {
                return;
            }
            item.Position = db.SingleById<PositionEntity>(item.PositionId);
        }

        public AdEntity Get(int id)
        {
            var model = db.Single<AdModel>("id=@0 AND start_at<=@1 AND end_at>@1", id, environment.Now);
            if (model is null)
            {
                throw new Exception("广告不存在");
            }
            WithPosition(model);
            return model;
        }

        public IList<FormattedAdModel> Load(string code)
        {
            var position = db.Single<PositionEntity>("WHERE code=@0 AND status=1", code);
            if (position is null)
            {
                return [];
            }
            if (position.SourceType > 0)
            {
                // 第三方广告
                // return position.Template;
            }
            var items = db.Fetch<AdEntity>("WHERE position_id=@0 AND status=1", position.Id);
            return Format(position, items, false);
        }

        public string Render(string code)
        {
            var position = db.Single<PositionEntity>("WHERE code=@0 AND status=1", code);
            if (position is null)
            {
                return string.Empty;
            }
            if (position.SourceType > 0)
            {
                return position.Template;
            }
            var items = db.Fetch<AdEntity>("WHERE position_id=@0 AND status=1", position.Id);
            var res = Format(position, items);
            return TemplateCompiler.Render(position.Template, res);
        }

        private static PositionEntity FormatSize(PositionEntity position)
        {
            if (position.AutoSize > 0)
            {
                position.Width = "100%";
                return position;
            }
            if (!string.IsNullOrWhiteSpace(position.Width) && Validator.IsNumeric(position.Width))
            {
                position.Width += "px";
            }
            if (!string.IsNullOrWhiteSpace(position.Height) && Validator.IsNumeric(position.Height))
            {
                position.Height += "px";
            }
            return position;
        }

        /**
         * @param array|AdPositionEntity position
         * @param array items
         * @param bool formatBody 是否预生成内容
         * @return array
         */
        public IList<FormattedAdModel> Format(PositionEntity position, IList<AdEntity> items, bool formatBody = true)
        {
            var data = new List<FormattedAdModel>();
            var formatted = FormatSize(position);
            var now = environment.Now;
            foreach (var item in items)
            {
                if (item.Status < 1 || (item.StartAt > 0 && item.StartAt > now)
                    || (item.EndAt > 0 && item.EndAt <= now))
                {
                    continue;
                }
                data.Add(new FormattedAdModel()
                {
                    Width = formatted.Width,
                    Height = formatted.Height,
                    Url = item.Url,
                    Type = item.Type,
                    Content = formatBody ? 
                    TemplateCompiler.RenderAd(item, formatted)
                     : item.Content
                });
            }
            return data;
        }
        public Page<AdModel> ManageList(string keywords, string position, int page = 1)
        {
            var id = 0;
            if (string.IsNullOrWhiteSpace(position))
            {
                id = db.FindScalar<int, PositionEntity>("id", "name=@0", position);
                if (id == 0)
                {
                    return new Page<AdModel>();
                }
            }
            return ManageList(keywords, id, page);
        }
        public Page<AdModel> ManageList(string keywords = "", 
            int position = 0, int page = 1)
        {
            var sql = new Sql();
            SearchHelper.Where(sql, "name", keywords);
            if (position > 0)
            {
                sql.Where("position_id=@0", position);
            }
            sql.Where("start_at<=@0 AND end_at>@0", environment.Now);
            sql.OrderBy("status DESC", "id Desc");
            var items = db.Page<AdModel>(1, 20, sql);
            WithPosition(items.Items);
            return items;
        }

        public AdEntity? ManageGet(int id)
        {
            return db.SingleById<AdEntity>(id);
        }

        public AdEntity ManageSave(AdForm data)
        {
            var model = data.Id > 0 ? db.SingleById<AdEntity>(data.Id) :
                new AdEntity();
            model.StartAt = data.StartAt;
            model.EndAt = data.EndAt;
            model.Name = data.Name;
            model.Type = data.Type;
            model.Url = data.Url;
            model.Content = data.Content;
            model.PositionId = data.PositionId;
            if (model.CreatedAt == 0)
            {
                model.CreatedAt = environment.Now;
            }
            model.UpdatedAt = environment.Now;
            db.TrySave(model);
            return model;
        }

        public void ManageRemove(int id)
        {
            db.Delete<AdEntity>(id);
        }

        public Page<PositionEntity> ManagePositionList(string keywords = "", int page = 1)
        {
            var sql = new Sql();
            SearchHelper.Where(sql, "name", keywords);
            sql.OrderBy("status DESC", "id Desc");
            return db.Page<PositionEntity>(page, 20, sql);
        }

        public PositionEntity? ManagePosition(int id)
        {
            return db.SingleById<PositionEntity>(id);
        }

        public PositionEntity ManagePositionSave(PositionForm data)
        {
            var model = data.Id > 0 ? db.SingleById<PositionEntity>(data.Id) :
                new PositionEntity();
            model.Template = data.Template;
            model.Status = data.Status;
            model.AutoSize = data.AutoSize;
            model.Width = data.Width;
            model.Height = data.Height;
            model.Code = data.Code;
            model.Name = data.Name;
            if (model.CreatedAt == 0)
            {
                model.CreatedAt = environment.Now;
            }
            model.UpdatedAt = environment.Now;
            db.TrySave(model);
            return model;
        }

    }
}
