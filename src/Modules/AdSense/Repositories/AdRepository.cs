using Microsoft.EntityFrameworkCore;
using NetDream.Modules.AdSense.Entities;
using NetDream.Modules.AdSense.Forms;
using NetDream.Modules.AdSense.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.AdSense.Repositories
{
    public class AdRepository(AdSenseContext db, IClientContext environment)
    {
        public const int TYPE_TEXT = 0;
        public const int TYPE_IMAGE = 1;
        public const int TYPE_HTML = 2;
        public const int TYPE_VIDEO = 3;

        public void ManagePositionRemove(int id)
        {
            db.Positions.Where(i => i.Id == id).ExecuteDelete();
            db.Ads.Where(i => i.PositionId == id).ExecuteDelete();
        }

        public IList<PositionEntity> PositionAll()
        {
            return db.Positions.ToArray();
        }

        public IList<FormattedAdModel> Banners()
        {
            return Load("banner");
        }

        public IList<FormattedAdModel> MobileBanners()
        {
            return Load("mobile_banner");
        }
        public AdEntity[] GetList(string keywords, string position)
        {
            var id = 0;
            if (string.IsNullOrWhiteSpace(position))
            {
                id = db.Positions.Where(i => i.Name == position).Select(i => i.Id).Single();
                if (id == 0)
                {
                    return [];
                }
            }
            return GetList(keywords, id);
        }
        public AdEntity[] GetList(string keywords = "", int position = 0)
        {
            var query = db.Ads.Include(i => i.Position).Where(i => i.StartAt <= environment.Now && i.EndAt > environment.Now);
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(i => i.Name.Contains(keywords));
            }
            if (position > 0)
            {
                query = query.Where(i => i.PositionId == position);
            }
            return query.ToArray();
        }


        public AdEntity Get(int id)
        {
            var model = db.Ads.Include(i => i.Position)
                .Where(i => i.Id == id && i.StartAt <= environment.Now && i.EndAt > environment.Now).Single();
            if (model is null)
            {
                throw new Exception("广告不存在");
            }
            return model;
        }

        public FormattedAdModel[] Load(string code)
        {
            var position = db.Positions.Where(i => i.Code == code && i.Status == 1).Single();
            if (position is null)
            {
                return [];
            }
            if (position.SourceType > 0)
            {
                // 第三方广告
                // return position.Template;
            }
            var items = db.Ads.Where(i => i.PositionId == position.Id && i.Status == 1).ToArray();
            return Format(position, items, false);
        }

        public string Render(string code)
        {
            var position = db.Positions.Where(i => i.Code == code && i.Status == 0).Single();
            if (position is null)
            {
                return string.Empty;
            }
            if (position.SourceType > 0)
            {
                return position.Template;
            }
            var items = db.Ads.Where(i => i.PositionId == position.Id && i.Status == 1).ToArray();
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
        public FormattedAdModel[] Format(PositionEntity position, IList<AdEntity> items, bool formatBody = true)
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
            return [..data];
        }
        public IPage<AdEntity> ManageList(string keywords, string position, int page = 1)
        {
            var id = 0;
            if (string.IsNullOrWhiteSpace(position))
            {
                id = db.Positions.Where(i => i.Name == position).Select(i => i.Id).Single();
                if (id == 0)
                {
                    return new Page<AdEntity>();
                }
            }
            return ManageList(keywords, id, page);
        }
        public IPage<AdEntity> ManageList(string keywords = "", 
            int position = 0, int page = 1)
        {
            return db.Ads.Include(i => i.Position).When(keywords, i => i.Name.Contains(keywords))
                .When(position > 0, i => i.PositionId == position)
                .Where(i => i.StartAt <= environment.Now && i.EndAt > environment.Now)
                .OrderByDescending(i => i.Status)
                .OrderByDescending(i => i.Id).ToPage(page);
        }

        public AdEntity? ManageGet(int id)
        {
            return db.Ads.Where(i => i.Id == id).Single();
        }

        public AdEntity ManageSave(AdForm data)
        {
            var model = data.Id > 0 ? db.Ads.Where(i => i.Id == data.Id).Single() :
                new AdEntity();
            if (model is null)
            {
                throw new Exception("id is error");
            }
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
            if (data.Id > 0)
            {
                db.Ads.Update(model);
            }
            else
            {
                db.Ads.Add(model);
            }
            db.SaveChanges();
            return model;
        }

        public void ManageRemove(int id)
        {
            db.Ads.Where(i => i.Id == id).ExecuteDelete();
        }

        public IPage<PositionEntity> ManagePositionList(string keywords = "", int page = 1)
        {
            return db.Positions.When(keywords, i => i.Name.Contains(keywords))
                .OrderByDescending(i => i.Status)
                .OrderByDescending(i => i.Id).ToPage(page);
        }

        public PositionEntity? ManagePosition(int id)
        {
            return db.Positions.Where(i => i.Id == id).Single();
        }

        public PositionEntity ManagePositionSave(PositionForm data)
        {
            var model = data.Id > 0 ? db.Positions.Where(i => i.Id == data.Id).Single() :
                new PositionEntity();
            if (model is null)
            {
                throw new Exception("id error");
            }
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
            if (data.Id > 0)
            {
                db.Positions.Update(model);
            } else
            {
                db.Positions.Add(model);
            }
            db.SaveChanges();
            return model;
        }

    }
}
