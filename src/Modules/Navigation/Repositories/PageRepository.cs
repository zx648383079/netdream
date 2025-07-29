using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Navigation.Entities;
using NetDream.Modules.Navigation.Forms;
using NetDream.Modules.Navigation.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Navigation.Repositories
{
    public class PageRepository(NavigationContext db,
        IUserRepository userStore,
        IClientContext client)
    {
        public IPage<PageListItem> GetList(QueryForm form)
        {
            var query = db.Pages.Search(form.Keywords, "title");
            if (form is PageQueryForm q)
            {
                query = query.When(q.User > 0, i => i.UserId == q.User)
                .When(q.Site > 0, i => i.SiteId == q.Site)
                .When(q.Domain, i => i.Link.Contains(q.Domain));
            }
            var items = query.OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            userStore.Include(items.Items);
            SiteRepository.Include(db, items.Items);
            var store = new KeywordRepository(db);
            foreach (var item in items.Items)
            {
                item.Keywords = store.GetTags(item.Id);
            }
            return items;
        }

        public IOperationResult<PageModel> Get(int id)
        {
            var model = db.Pages.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<PageModel>("数据有误");
            }
            return OperationResult.Ok(Convert(model));
        }

        public IOperationResult<PageEntity> Save(PageForm data, bool checkSite = true)
        {
            var model = data.Id > 0 ? db.Pages.Where(i => i.Id == data.Id)
                .SingleOrDefault() :
                new PageEntity();
            if (model is null)
            {
                return OperationResult.Fail<PageEntity>("id error");
            }
            model.Title = data.Title;
            model.Description = data.Description;
            model.Thumb = data.Thumb;
            model.Link = data.Link;
            model.SiteId = data.SiteId;
            model.Score = data.Score;
            if (model.UserId <= 0)
            {
                model.UserId = client.UserId;
            }
            if (Exist(model))
            {
                return OperationResult<PageEntity>.Fail("网页已存在");
            }
            if (checkSite)
            {
                var site = new SiteRepository(db, null, null).FindIdByLink(model.Link);
                if (site is not null)
                {
                    model.SiteId = site.Id;
                    model.Score = site.Score;
                }
            }
            db.Pages.Save(model, client.Now);
            db.SaveChanges();

            new KeywordRepository(db).BindTag(
                model.Id,
                data.Keywords.Split(',').Where(i => !string.IsNullOrWhiteSpace(i)).ToArray()
            );
            return OperationResult.Ok(model);
        }

        public bool Exist(PageEntity model)
        {
            return db.Pages.Where(i => i.Link == model.Link && i.Id != model.Id).Any();
        }

        public IOperationResult<PageEntity> Check(string link, int id = 0)
        {
            var model = db.Pages.Where(i => i.Link == link && i.Id != id).FirstOrDefault();
            if (model == null)
            {
                return OperationResult<PageEntity>.Fail("not exist");
            }
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Pages.Where(i => i.Id == id).ExecuteDelete();
            db.PageKeywords.Where(i => i.PageId == id).ExecuteDelete();
        }

        public IOperationResult<PageEntity> CrawlSave(PageCrawlForm data)
        {
            return Save(new PageForm()
            {
                Title = data.Title,
                Description = data.Description,
                Thumb = data.Thumb,
                Link = data.Link,
                Keywords = data.Keywords
            });
        }

        public PageModel Convert(PageEntity entity)
        {
            var res = entity.CopyTo<PageModel>();
            userStore.Include(res);
            SiteRepository.Include(db, [res]);
            res.Keywords = new KeywordRepository(db).GetTags(res.Id);
            return res;
        }
    }
}
