using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Document.Entities;
using NetDream.Modules.Document.Forms;
using NetDream.Modules.Document.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Document.Repositories
{
    public class ProjectRepository(DocumentContext db, IClientContext client)
    {
        public const byte STATUS_PUBLIC = 0; // 公开
        public const byte STATUS_PRIVATE = 1; // 私有

        public const byte TYPE_NONE = 0;
        public const byte TYPE_API = 1;
        public CommentProvider Comment()
        {
            return new CommentProvider(db, client);
        }

        public ActionLogProvider Log()
        {
            return new ActionLogProvider(db, client);
        }

        public bool IsSelf(int projectId)
        {
            return db.Projects.Where(i => i.Id == projectId && i.UserId == client.UserId).Any();
        }

        public IPage<ProjectEntity> GetList(QueryForm form, int type = 0)
        {
            var query = db.Projects.Search(form.Keywords, "name");
            if (client.UserId == 0)
            {
                query = query.Where(i => i.Status == STATUS_PUBLIC);
            }
            else
            {
                query = query.Where(i => i.Status == STATUS_PUBLIC || i.UserId == client.UserId);
            }
            return query
            .When(type > 0, i => i.Type == type - 1)
            .OrderByDescending(i => i.Id).ToPage(form);
        }

        public IPage<ProjectEntity> GetSelfList(QueryForm form, int type = 0)
        {
            return db.Projects.Search(form.Keywords, "name")
                .Where(i => i.UserId == client.UserId)
                .When(type > 0, i => i.Type == type - 1)
                .OrderByDescending(i => i.Id).ToPage(form);
        }

        /**
         * @param int id
         * @return ProjectModel
         * @throws Exception
         */
        public IOperationResult<ProjectEntity> Get(int id)
        {
            var query = db.Projects.Where(i => i.Id == id);
            if (client.UserId == 0)
            {
                query = query.Where(i => i.Status == STATUS_PUBLIC);
            }
            else
            {
                query = query.Where(i => i.Status == STATUS_PUBLIC || i.UserId == client.UserId);
            }
            var model = query.SingleOrDefault();
            if (model is null)
            {
                return OperationResult<ProjectEntity>.Fail("项目文档不存在");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<ProjectEntity> GetSelf(int id)
        {
            var model = db.Projects.Where(i => i.Id == id && i.UserId == client.UserId)
                .SingleOrDefault();
            if (model is null)
            {
                return OperationResult<ProjectEntity>.Fail("项目文档不存在");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<ProjectEntity> Save(ProjectForm data)
        {
            var model = data.Id > 0 ? db.Projects.Where(i => i.Id == data.Id && i.UserId == client.UserId)
                .SingleOrDefault() : new ProjectEntity();
            if (model is null)
            {
                return OperationResult<ProjectEntity>.Fail("项目文档不存在");
            }
            model.Name = data.Name;
            model.Description = data.Description;
            model.CatId = data.CatId;
            model.Cover = data.Cover;
            model.Type = data.Type;
            model.Environment = data.Environment;
            model.UserId = client.UserId;
            db.Projects.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult RemoveSelf(int id)
        {
            var res = GetSelf(id);
            if (!res.Succeeded)
            {
                return res;
            }
            db.Projects.Remove(res.Result);
            db.Versions.Where(i => i.ProjectId == id).ExecuteDelete();
            db.Pages.Where(i => i.ProjectId == id).ExecuteDelete();
            var apiId = db.Apies.Where(i => i.ProjectId == id)
               .Select(i => i.Id).ToArray();
            if (apiId.Length == 0)
            {
                db.SaveChanges();
                return OperationResult.Ok();
            }
            db.Fields.Where(i => apiId.Contains(i.ApiId)).ExecuteDelete();
            db.Apies.Where(i => i.ProjectId == id).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult CreateVersion(int id, int srcVersion, string version)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                return OperationResult.Fail("请输入正确的版本号");
            }
            var exist = db.Versions.Where(i => i.ProjectId == id && i.Name == version)
                .Any();
            if (exist)
            {
                return OperationResult.Fail("版本号已存在");
            }
            var res = GetSelf(id);
            if (!res.Succeeded)
            {
                return res;
            }
            var project = res.Result;
            var model = new ProjectVersionEntity()
            {
                ProjectId = id,
                Name = version
            };
            db.Versions.Save(model, client.Now);
            db.SaveChanges();
            if (model.Id == 0)
            {
                return OperationResult.Fail("创建版本号失败");
            }
            if (project.Type < 1)
            {
                CopyPageVersion(project.Id, srcVersion, model.Id);
                return OperationResult.Ok(model);
            }
            CopyApiVersion(project.Id, srcVersion, model.Id);
            return OperationResult.Ok(model);
        }

        private void CopyPageVersion(int project, int srcVersion, 
            int distVersion)
        {
            var mapId = new Dictionary<int, int>();
            var items = db.Pages.Where(i => i.ProjectId == project && i.VersionId == srcVersion)
                .OrderBy(i => i.ParentId).ToArray();
            foreach (var item in items)
            {
                var target = new PageEntity()
                {
                    VersionId = distVersion,
                    ProjectId = project,
                    Name = item.Name,
                    Content = item.Content,
                    Type = item.Type,
                    UpdatedAt = item.UpdatedAt,
                    CreatedAt = item.CreatedAt,
                };
                if (item.ParentId > 0 && mapId.TryGetValue(item.ParentId, out var pid))
                {
                    target.ParentId = pid;
                }
                db.Pages.Add(target);
                db.SaveChanges();
                mapId.Add(item.Id, target.Id);
            }
        }

        private void CopyApiVersion(int project, 
            int srcVersion, int distVersion)
        {
            var mapId = new Dictionary<int, int>();
            var items = db.Apies.Where(i => i.ProjectId == project && i.VersionId == srcVersion)
                .OrderBy(i => i.ParentId).ToArray();
            foreach (var item in items)
            {
                var target = new ApiEntity()
                {
                    VersionId = distVersion,
                    ProjectId = project,
                    Type = item.Type,
                    CreatedAt = item.CreatedAt,
                    Description = item.Description,
                    Name = item.Name,
                    Method = item.Method,
                    Uri = item.Uri,
                    UpdatedAt = item.UpdatedAt,
                };
                if (item.ParentId > 0 && mapId.TryGetValue(item.ParentId, out var pid))
                {
                    target.ParentId = pid;
                }
                db.Apies.Add(target);
                db.SaveChanges();
                mapId.Add(item.Id, target.Id);
            }
            if (mapId.Count == 0)
            {
                return;
            }
            var fieldMap = new Dictionary<int, int>();
            var items2 = db.Fields.Where(i => mapId.Keys.Contains(i.ApiId))
                .OrderBy(i => i.ParentId).ToArray();
            foreach (var item in items2)
            {
                var target = new FieldEntity()
                {
                    ApiId = mapId[item.ApiId],
                    Name = item.Name,
                    Title = item.Title,
                    Mock = item.Mock,
                    Kind = item.Kind,
                    DefaultValue = item.DefaultValue,
                    Remark = item.Remark,
                    IsRequired = item.IsRequired,
                    Type = item.Type,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                };
                if (item.ParentId > 0 && fieldMap.TryGetValue(item.ParentId, out var pid))
                {
                    item.ParentId = pid;
                }
                db.Fields.Add(target);
                db.SaveChanges();
                fieldMap.Add(item.Id, target.Id);
            }
        }

        public void VersionRemove(int project, int id = 0)
        {
            if (id > 0)
            {
                db.Versions.Where(i => i.Id == id).ExecuteDelete();
            }
            db.Pages.Where(i => i.ProjectId == project && i.VersionId == id)
                .ExecuteDelete();
            var apiId = db.Apies.Where(i => i.ProjectId == project && i.VersionId == id)
                .Select(i => i.Id).ToArray();
            if (apiId.Length == 0)
            {
                db.SaveChanges();
                return;
            }
            db.Fields.Where(i => apiId.Contains(i.ApiId)).ExecuteDelete();
            db.Apies.Where(i => i.ProjectId == project && i.VersionId == id).ExecuteDelete();
            db.SaveChanges();
        }

        public ListLabelItem[] All()
        {
            return db.Projects.OrderBy(i => i.Id)
                .Select(i => new ListLabelItem(i.Id, i.Name))
                .ToArray();
        }

        public ListLabelItem[] AllSelf()
        {
            return db.Projects.Where(i => i.UserId == client.UserId)
                .OrderBy(i => i.Id)
                .Select(i => new ListLabelItem(i.Id, i.Name))
                .ToArray();
        }

        public ListLabelItem[] VersionAll(int id)
        {
            var items = db.Versions.Where(i => i.ProjectId == id)
                .OrderBy(i => i.Id)
                .Select(i => new ListLabelItem(i.Id, i.Name)).ToList();
            items.Insert(0, new ListLabelItem(0, "main"));
            return [..items];
        }

        public ITreeItem[] Catalog(int id, int version)
        {
            var res = Get(id);
            if (!res.Succeeded)
            {
                return [];
            }
            return res.Result.Type > 0 ? new ApiRepository(db, client).Tree(id, version)
                : new PageRepository(db, client).Tree(id, version);
        }

        public bool CanOpen(int id)
        {
            var query = db.Projects.Where(i => i.Id == id);
            if (client.UserId == 0)
            {
                query = query.Where(i => i.Status == STATUS_PUBLIC);
            } else
            {
                query = query.Where(i => i.Status == STATUS_PUBLIC || i.UserId == client.UserId);
            }
            return query.Any();
        }

        public IOperationResult<IPageModel> Page(int project, int id)
        {
            var res = Get(project);
            if (!res.Succeeded)
            {
                return OperationResult<IPageModel>.Fail(res.Message);
            }
            var model = res.Result;
            if (model.Type < 1)
            {
                var pageStore = new PageRepository(db, client);
                var page = pageStore.Get(id);
                if (!page.Succeeded)
                {
                    return OperationResult<IPageModel>.Fail("文档错误");
                }
                return pageStore.GetRead(page.Result);
            }
            var apiStore = new ApiRepository(db, client);
            var api = apiStore.Get(id);
            if (!api.Succeeded)
            {
                return OperationResult<IPageModel>.Fail("文档错误");
            }
            return apiStore.GetRead(api.Result);
        }

        public ListLabelItem[] Suggest(string keywords)
        {
            return db.Projects.Search(keywords, "name")
                .Where(i => i.Status == STATUS_PUBLIC)
                .Take(4)
                .Select(i => new ListLabelItem(i.Id, i.Name)).ToArray();
        }

    }
}
