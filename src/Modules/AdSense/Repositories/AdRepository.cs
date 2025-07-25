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

        public PositionEntity[] PositionAll()
        {
            return db.Positions.ToArray();
        }

        public FormattedAdModel[] Banners()
        {
            return Load("banner");
        }

        public FormattedAdModel[] MobileBanners()
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


        public IOperationResult<AdEntity> Get(int id)
        {
            var model = db.Ads.Include(i => i.Position)
                .Where(i => i.Id == id && i.StartAt <= environment.Now && i.EndAt > environment.Now).Single();
            if (model is null)
            {
                return OperationResult.Fail<AdEntity>("广告不存在");
            }
            return OperationResult.Ok(model);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="items"></param>
        /// <param name="formatBody">是否预生成内容</param>
        /// <returns></returns>
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
        public IPage<AdEntity> ManageList(AdQueryForm form)
        {
            var id = 0;
            var positionId = Validator.IsInt(form.Position) ? int.Parse(form.Position) : 0;
            if (positionId == 0 && !string.IsNullOrWhiteSpace(form.Position))
            {
                positionId = db.Positions.Where(i => i.Name == form.Position).Select(i => i.Id).Single();
                if (id == 0)
                {
                    return new Page<AdEntity>();
                }
            }
            return db.Ads.Include(i => i.Position).When(form.Keywords, i => i.Name.Contains(form.Keywords))
                .When(positionId > 0, i => i.PositionId == positionId)
                .Where(i => i.StartAt <= environment.Now && i.EndAt > environment.Now)
                .OrderByDescending(i => i.Status)
                .ThenByDescending(i => i.Id).ToPage(form);
        }

        public IOperationResult<AdEntity> ManageGet(int id)
        {
            var res = db.Ads.Where(i => i.Id == id).Single();
            if (res is null)
            {
                return OperationResult<AdEntity>.Fail("id is error");
            }
            return OperationResult.Ok(res);
        }

        public IOperationResult<AdEntity> ManageSave(AdForm data)
        {
            var model = data.Id > 0 ? db.Ads.Where(i => i.Id == data.Id).Single() :
                new AdEntity();
            if (model is null)
            {
                return OperationResult<AdEntity>.Fail("id is error");
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
            return OperationResult.Ok(model);
        }

        public void ManageRemove(int id)
        {
            db.Ads.Where(i => i.Id == id).ExecuteDelete();
        }

        public IPage<PositionEntity> ManagePositionList(QueryForm form)
        {
            return db.Positions.When(form.Keywords, i => i.Name.Contains(form.Keywords))
                .OrderByDescending(i => i.Status)
                .ThenByDescending(i => i.Id).ToPage(form);
        }

        public IOperationResult<PositionEntity> ManagePosition(int id)
        {
            var res = db.Positions.Where(i => i.Id == id).Single();
            if (res is null)
            {
                return OperationResult<PositionEntity>.Fail("id is error");
            }
            return OperationResult.Ok(res);
        }

        public IOperationResult<PositionEntity> ManagePositionSave(PositionForm data)
        {
            var model = data.Id > 0 ? db.Positions.Where(i => i.Id == data.Id).Single() :
                new PositionEntity();
            if (model is null)
            {
                return OperationResult<PositionEntity>.Fail("id is error");
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
            return OperationResult.Ok(model);
        }

    }
}
