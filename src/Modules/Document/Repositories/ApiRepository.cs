using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class ApiRepository(DocumentContext db, IClientContext client)
    {
        public const byte KIND_REQUEST = 1;
        public const byte KIND_RESPONSE = 2;
        public const byte KIND_HEADER = 3;
        public ITreeItem[] Tree(int project,int version = 0) {
            var data = db.Apies.Where(i => i.ProjectId == project && i.VersionId == version)
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

        public IOperationResult<ApiEntity> Get(int id) 
        {
            var model = db.Apies.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<ApiEntity>("文档不存在");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<IPageModel> MangerGet(int id)
        {
            var model = db.Apies.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<IPageModel>("文档不存在");
            }
            return GetRead(model);
        }

        public IOperationResult<ApiEntity> GetSelf(int id) 
        {
            var res = Get(id);
            if (res.Succeeded && !new ProjectRepository(db, client).IsSelf(res.Result.ProjectId))
            {
                return OperationResult.Fail<ApiEntity>("文档不存在");
            }
            return res;
        }

        public IOperationResult<IPageModel> EditSelf(int id)
        {
            var model = db.Apies.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<IPageModel>("文档不存在");
            }
            if (!new ProjectRepository(db, client).IsSelf(model.ProjectId))
            {
                return OperationResult.Fail<IPageModel>("文档不存在");
            }
            return GetRead(model);
        }

        public IOperationResult<ApiEntity> Save(ApiForm data) 
        {
            var model = data.Id > 0 ? db.Apies.Where(i => i.Id == data.Id).SingleOrDefault() : new ApiEntity();
            if (model is null)
            {
                return OperationResult.Fail<ApiEntity>("文档不存在");
            }
            if (data.Id == 0)
            {
                model.ProjectId = data.ProjectId;
            }
            if (!new ProjectRepository(db, client).IsSelf(model.ProjectId))
            {
                return OperationResult.Fail<ApiEntity>("文档不存在");
            }
            model.ParentId = data.ParentId;
            model.Description = data.Description;
            model.VersionId = data.VersionId;
            model.Name = data.Name;
            model.Type = data.Type;
            db.Apies.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult<ApiEntity> SaveWeb(ApiForm data, int[] fieldIds) 
        {
            var res = Save(data);
            if (res.Succeeded && data.Id < 1 && fieldIds.Length > 0) {
                db.Fields.Where(i => fieldIds.Contains(i.Id))
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.ApiId, 
                    res.Result.Id));
            }
            return res;
        }

        public IOperationResult<ApiEntity> SaveApi(ApiForm data) {
            var res = Save(data);
            if (!res.Succeeded)
            {
                return res;
            }
            var model = res.Result;
            var oldItems = Array.Empty<FieldEntity>();
            if (data.Id > 0) {
                oldItems = db.Fields.Where(i => i.ApiId == model.Id).ToArray();
            }
            var doId = new List<int>();
            int findId(FieldForm data)
            {
                if (oldItems.Length == 0)
                {
                    return 0;
                }
                if (data.Id > 0)
                {
                    return data.Id;
                }
                foreach (var item in oldItems)
                {
                    if (doId.Contains(item.Id))
                    {
                        continue;
                    }
                    if (item.Name == data.Name && item.ParentId == data.ParentId && item.Kind == data.Kind)
                    {
                        return item.Id;
                    }
                }
                return 0;
            }
            void saveField(byte kind, int parent_id, FieldForm item) {
                if (string.IsNullOrWhiteSpace(item.Name)) {
                    return;
                }
                item.Kind = kind;
                item.ParentId = parent_id;
                var id = SaveField(model.Id, findId(item), parent_id, item);
                if (id < 1) {
                    return;
                }
                doId.Add(id);
                saveFieldArray(kind, id, item.Children);
            };
            void saveFieldArray(byte kind, int parent_id, FieldForm[]? items)
            {
                if (items is null || items.Length == 0)
                {
                    return;
                }
                foreach (var it in items)
                {
                    saveField(kind, parent_id, it);
                }
            }
            saveFieldArray(KIND_HEADER, 0, data.Header);
            saveFieldArray(KIND_REQUEST, 0, data.Request);
            saveFieldArray(KIND_RESPONSE, 0, data.Response);
            var del = new List<int>();
            foreach (var item in oldItems)  {
                if (doId.Contains(item.Id)) 
                {
                    continue;
                }
                del.Add(item.Id);
            }
            if (del.Count > 0) {
                db.Fields.Where(i => del.Contains(i.Id)).ExecuteDelete();
            }
            return res;
        }

        private int SaveField(int apiID, int id, int parentId, FieldForm data) 
        {
            if (id > 0) {
                db.Fields.Where(i => i.Id == id)
                    .ExecuteUpdate(setters => 
                        setters.SetProperty(i => i.ApiId, apiID)
                        .SetProperty(i => i.ParentId, parentId)
                        .SetProperty(i => i.UpdatedAt, client.Now)
                        .SetProperty(i => i.Name, data.Name)
                        .SetProperty(i => i.Title, data.Title)
                        .SetProperty(i => i.Type, data.Type)
                        .SetProperty(i => i.Kind, data.Kind)
                        .SetProperty(i => i.DefaultValue, data.DefaultValue)
                        .SetProperty(i => i.IsRequired, data.IsRequired)
                        .SetProperty(i => i.Mock, data.Mock)
                        .SetProperty(i => i.Remark, data.Remark)
                        );
                db.SaveChanges();
                return id;
            } else {
                var model = new FieldEntity()
                {
                    ApiId = apiID,
                    ParentId = parentId,
                    UpdatedAt = client.Now,
                    CreatedAt = client.Now,
                    Name = data.Name,
                    Title = data.Title,
                    Type = data.Type,
                    Kind = data.Kind,
                    DefaultValue = data.DefaultValue,
                    IsRequired  = data.IsRequired,
                    Mock = data.Mock,
                    Remark = data.Remark,
                };
                db.Fields.Add(model);
                db.SaveChanges();
                return model.Id;
            }
            
        }

        public IOperationResult RemoveSelf(int id) 
        {
            var model = db.Apies.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail("文档不存在");
            }
            if (!new ProjectRepository(db, client).IsSelf(model.ProjectId))
            {
                return OperationResult.Fail("文档不存在");
            }
            var apiId = db.Apies.Where(i => i.Id == id || i.ParentId == id).Select(i => i.Id).ToArray();
            db.Fields.Where(i => apiId.Contains(i.ApiId)).ExecuteDelete();
            db.Apies.Where(i => apiId.Contains(i.Id)).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public (FieldEntity[], FieldEntity[], FieldTreeItem[]) FieldList(int apiId) {
            var data = db.Fields.Where(i => i.ApiId == apiId).ToArray();
            var header = data.Where(i => i.Kind == KIND_HEADER).ToArray();
            var request = data.Where(i => i.Kind == KIND_REQUEST).ToArray();
            var response = TreeHelper.Sort(data.Where(i => i.Kind == KIND_RESPONSE)
                .Select(i => i.CopyTo<FieldTreeItem>()).ToArray()); ;
            return (header, request, response);
        }

        public IOperationResult<FieldEntity> FieldSelf(int id) 
        {
            var model = db.Fields.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<FieldEntity>.Fail("数据错误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<FieldEntity> FieldSave(FieldForm data) 
        {
            var model = data.Id > 0 ? db.Fields.Where(i => i.Id == data.Id).SingleOrDefault() 
                : new FieldEntity();
            if (model is null)
            {
                return OperationResult<FieldEntity>.Fail("id is error");
            }
            model.Name = data.Name;
            model.Type = data.Type;
            model.Title = data.Title;
            model.DefaultValue = data.DefaultValue;
            model.Mock = data.Mock;
            model.IsRequired = data.IsRequired;
            model.Remark = data.Remark;
            model.ApiId = data.ApiId;
            db.Fields.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void FieldRemove(int id) 
        {
            db.Fields.Where(i => i.Id == id || i.ParentId == id).ExecuteDelete();
            db.SaveChanges();
        }

        public bool CanOpen(int id) 
        {
            var project_id = db.Apies.Where(i => i.Id == id)
                .Select(i => i.ProjectId).FirstOrDefault();
            return project_id > 0 && new ProjectRepository(db, client)
                .CanOpen(project_id);
        }
        public IOperationResult<IPageModel> GetRead(int id)
        {
            var model = db.Apies.Where(i => i.Id == id).SingleOrDefault();
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
        public IOperationResult<IPageModel> GetRead(ApiEntity model) 
        {
            var (header, request, response) = FieldList(model.Id);
            var example = new MockRepository(db, client).GetDefaultData(model.Id);
            var res = model.CopyTo<ApiModel>();
            res.Header = header;
            res.Request = request;
            res.Response = response;
            return OperationResult.Ok<IPageModel>(res);
        }
        
    }
}
