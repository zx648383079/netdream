using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Document.Entities;
using NetDream.Modules.Document.Forms;
using NetDream.Modules.Document.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Document.Repositories
{
    public class PageRepository(DocumentContext db, IClientContext client)
    {
        public ITreeItem[] Tree(int project, int version = 0)
        {
            var data = db.Pages.Where(i => i.ProjectId == project && i.VersionId == version)
                .OrderBy(i => i.Id)
                .Select(i => new PageTreeItem()
                {
                    Id = i.Id,
                    ParentId = i.ParentId,
                    Name = i.Name,
                    Type = i.Type,
                }).ToArray();
            return TreeHelper.Create(data);
        }

        public IOperationResult<PageEntity> Get(int id)
        {
            var model = db.Pages.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<PageEntity>("文档不存在");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<PageEntity> GetSelf(int id)
        {
            var res = Get(id);
            if (res.Succeeded && !new ProjectRepository(db, client).IsSelf(res.Result.ProjectId))
            {
                return OperationResult.Fail<PageEntity>("文档不存在");
            }
            return res;
        }

        public IOperationResult<PageEntity> Save(PageForm data)
        {
            var model = data.Id > 0 ? db.Pages.Where(i => i.Id == data.Id).SingleOrDefault() : new PageEntity();
            if (model is null)
            {
                return OperationResult.Fail<PageEntity>("文档不存在");
            }
            if (data.Id == 0)
            {
                model.ProjectId = data.ProjectId;
            }
            if (!new ProjectRepository(db, client).IsSelf(model.ProjectId))
            {
                return OperationResult.Fail<PageEntity>("文档不存在");
            }
            model.Name = data.Name;
            model.ParentId = data.ParentId;
            model.VersionId = data.VersionId;
            model.Content = data.Content;
            model.Type = data.Type;
            db.Pages.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult RemoveSelf(int id)
        {
            var model = db.Pages.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail("id is error");
            }
            if (new ProjectRepository(db, client).CanOpen(model.ProjectId))
            {
                return OperationResult.Fail("无权限浏览");
            }
            db.Pages.Where(i => i.Id == id || i.ParentId == id).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }
        public IOperationResult<IPageModel> GetRead(int id)
        {
            var model = db.Pages.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<IPageModel>.Fail("id is error");
            }
            if (new ProjectRepository(db, client).CanOpen(model.ProjectId))
            {
                return OperationResult<IPageModel>.Fail("无权限浏览");
            }
            return GetRead(model);
        }
        public IOperationResult<IPageModel> GetRead(PageEntity model)
        {
            var res = model.CopyTo<PageModel>();
            res.Content = Markdig.Markdown.ToHtml(model.Content);
            return OperationResult.Ok<IPageModel>(res);
        }
    }
}
