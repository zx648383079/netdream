using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Navigation.Entities;
using NetDream.Modules.Navigation.Forms;
using NetDream.Modules.Navigation.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Navigation.Repositories
{
    public class SiteRepository(NavigationContext db, 
        IUserRepository userStore,
        IClientContext client)
    {
        public TagProvider Tag()
        {
            return new TagProvider(db);
        }

        public IPage<SiteListItem> GetList(SiteQueryForm form)
        {
            return db.Sites.Search(form.Keywords, "name", "domain")
                .When(form.Category > 0, i => i.CatId == form.Category)
                .When(form.Domain, i => i.Domain == form.Domain)
                .OrderByDescending(i => i.Score)
                .ToPage(form, i => i.SelectAs());
        }

        public IOperationResult<SiteModel> Get(int id)
        {
            var model = db.Sites.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<SiteModel>.Fail("id is error");
            }
            var res = model.CopyTo<SiteModel>();
            res.Tags = Tag().GetTags(model.Id);
            return OperationResult.Ok(res);
        }


        public ITreeItem[] Categories()
        {
            var items = db.Categories.OrderByDescending(i => i.Id)
                .SelectAs()
                .ToArray();
            return TreeHelper.Create(items);
        }

        public SiteEntity[] Recommend(int category)
        {
            return db.Sites.Where(i => i.CatId == category && i.TopType > 0)
                .OrderByDescending(i => i.TopType)
                .ThenBy(i => i.Id).ToArray();
        }

        public CategoryModel[] RecommendGroup(int category = 0)
        {
            var catItems = db.Categories.Where(i => i.Id == category || i.ParentId == category)
                .OrderBy(i => i.ParentId)
                .ThenBy(i => i.Id)
                .ToArray();
            if (catItems.Length == 0)
            {
                return [];
            }
            var catId = catItems.Select(i => i.Id).ToArray();
            var items = db.Sites.Where(i => catId.Contains(i.CatId) && i.TopType > 0)
                .OrderByDescending(i => i.TopType)
                .ThenBy(i => i.Id).SelectAs().ToArray();
            if (items.Length == 0)
            {
                return [];
            }
            return catItems.Select(i => {
                var next = i.CopyTo<CategoryModel>();
                next.Items = items.Where(j => i.Id == j.CatId).ToArray();
                return next;
            }).ToArray();
        }

        public IPage<SiteListItem> MangerList(SiteQueryForm form)
        {
            var items = db.Sites.Search(form.Keywords, "name")
                .When(form.Category > 0, i => i.CatId == form.Category)
                .When(form.User > 0, i => i.UserId == form.User)
                .When(form.Domain, i => i.Domain == form.Domain)
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            CategoryRepository.Include(db, items.Items);
            var tagStore = Tag();
            foreach (var item in items.Items)
            {
                item.Tags = tagStore.GetTags(item.Id);
            }
            return items;
        }

        public IOperationResult<SiteEntity> Save(SiteForm data)
        {
            var model = data.Id > 0 ? db.Sites.Where(i => i.Id == data.Id)
                .SingleOrDefault() :
                new SiteEntity();
            if (model is null)
            {
                return OperationResult.Fail<SiteEntity>("id error");
            }
            model.Name = data.Name;
            model.Logo = data.Logo;
            model.Description = data.Description;
            model.CatId = data.CatId;
            model.Domain = data.Domain.Trim('/');
            model.Schema = data.Schema;
            if (model.UserId < 1)
            {
                model.UserId = client.UserId;
            }
            if (Exist(model))
            {
                return OperationResult.Fail<SiteEntity>("站点已存在！");
            }
            db.Sites.Save(model, client.Now);
            db.SaveChanges();
            Tag().BindTag(model.Id, data.Tags);
            if (data.Id < 1 && data.AlsoPage > 0)
            {
                new PageRepository(db, userStore, client).Save(new PageForm()
                {
                    Title = model.Name,
                    Description = model.Description,
                    Thumb = model.Logo,
                    Link = $"{model.Schema}://{model.Domain}",
                    SiteId = model.Id,
                    Score = 80,
                    Keywords = data.Keywords
                });
                
            }
            return OperationResult.Ok(model);
        }

        public bool Exist(SiteEntity model)
        {
            return db.Sites.Where(i => i.Schema == model.Schema && i.Domain == model.Domain
                && i.Id != model.Id).Any();
        }

        public IOperationResult<SiteEntity> Check(string domain, int id = 0)
        {
            var model = db.Sites.Where(i => i.Domain == domain && i.Id != id).FirstOrDefault();
            if (model is null)
            {
                return OperationResult<SiteEntity>.Fail("not exist");
            }
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Sites.Where(i => i.Id == id).ExecuteDelete();
            Tag().RemoveLink(id);
        }

        public SiteEntity? FindIdByLink(string link)
        {
            if (string.IsNullOrWhiteSpace(link) || !Uri.TryCreate(link, UriKind.Absolute, out var u))
            {
                return null;
            }
            var host = u.Host;
            return db.Sites.Where(i => i.Domain == host).OrderByDescending(i => i.Id).FirstOrDefault();
        }

        public IOperationResult<SiteEntity> Scoring(SiteScoringForm data)
        {
            var model = db.Sites.Where(i => i.Id == data.Id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<SiteEntity>("数据有误");
            }
            var log = new SiteScoringLogEntity()
            {
                SiteId = model.Id,
                UserId = client.UserId,
                Score = data.Score,
                LastScore = model.Score,
                ChangeReason = data.ChangeReason
            };
            db.SiteScoringLogs.Save(log, client.Now);
            model.Score = data.Score;
            db.Sites.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IPage<ScoringLogListItem> GetScoreLog(ScoreQueryForm form)
        {
            var items = db.SiteScoringLogs.Where(i => i.SiteId == form.Site)
                .OrderByDescending(i => i.CreatedAt)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            return items;
        }

        internal static void Include(NavigationContext db, IWithSiteModel[] items)
        {
            var idItems = items.Select(item => item.SiteId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Sites.Where(i => idItems.Contains(i.Id))
                .Select(i => new SiteLabelItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Schema = i.Schema,
                    Domain = i.Domain,
                    Logo = i.Logo,
                })
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.SiteId > 0 && data.TryGetValue(item.SiteId, out var res))
                {
                    item.Site = res;
                }
            }
        }
    }
}
