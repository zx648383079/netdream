using Microsoft.EntityFrameworkCore;
using NetDream.Modules.ResourceStore.Entities;
using NetDream.Modules.ResourceStore.Forms;
using NetDream.Modules.ResourceStore.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NetDream.Modules.ResourceStore.Repositories
{
    public class ResourceRepository(ResourceContext db, 
        IClientContext client, IUserRepository userStore)
    {
        public const byte LOG_TYPE_RES = 0;
        public const byte LOG_ACTION_BUY = 66;
        public const byte LOG_ACTION_DOWNLOAD = 1;
        public TagProvider Tag()
        {
            return new TagProvider(db);
        }
        public ActionLogProvider Log()
        {
            return new ActionLogProvider(db, client);
        }
        public CommentProvider Comment()
        {
            return new CommentProvider(db, client);
        }
        public ScoreProvider Score()
        {
            return new ScoreProvider(db, client);
        }

        public IPage<ResourceListItem> GetList(QueryForm form, 
            int category = 0, int user = 0, string tag = "", 
            string sort = "created_at", string order = "desc")
        {
            var query = GetListQuery(form.Keywords, category, user, tag, sort, order);
            if (query is null)
            {
                return new Page<ResourceListItem>();
            }
            var res = query.ToPage(form);
            userStore.Include(res.Items);
            CategoryRepository.Include(db, res.Items);
            return res;
        }

        public IQueryable<ResourceListItem>? GetListQuery(string keywords = "", 
            int category = 0, int user = 0, string tag = "", 
            string sort = "created_at", string order = "desc")
        {
            var categories = category > 0 ? new CategoryRepository(db).GetAllChildrenId(category) : [];
            var query = db.Resources.When(category > 0, i => categories.Contains(i.CatId))
            .When(user > 0, i => i.UserId == user)
            .Search(keywords, "title");
            if (!string.IsNullOrWhiteSpace(tag))
            {
                var tags = Tag().SearchTag(tag);
                if (tags.Count == 0)
                {
                    return null;
                }
                query = query.Where(i => tags.Contains(i.Id));
            }
            switch (sort)
            {
                case "new":
                    query = query.OrderByDescending(i => i.CreatedAt);
                    break;
                case "trending":
                    query = query.Where(i => i.IsCommercial == 1)
                        .OrderBy(i => i.Price);
                    break;
                case "free":
                    query = query.Where(i => i.Price == 0);
                    break;
                case "hot":
                    query = query.OrderByDescending(i => i.DownloadCount);
                    break;
                default:
                    (sort, order) = SearchHelper.CheckSortOrder(sort, order, [
                        "id", "price", "score", "created_at", "download_count", "view_count", "comment_count"
                    ]);
                    query = query.OrderBy<ResourceEntity, int>(sort, order);
                    break;
            }
            return AsList(query);
        }

        public ResourceListItem[] GetLimitList(int count, 
            string keywords = "", int category = 0, int user = 0, 
            string tag = "", string sort = "created_at", string order = "desc")
        {
            var query = GetListQuery(keywords, category, user, tag, sort, order);
            if (query is null)
            {
                return [];
            }
            var res = query.Take(count).ToArray();
            userStore.Include(res);
            CategoryRepository.Include(db, res);
            return res;
        }

        public IOperationResult<ResourceEntity> Get(int id)
        {
            var model = db.Resources.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<ResourceEntity>.Fail("资源不存在");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<ResourceModel> GetEdit(int id)
        {
            var model = db.Resources.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<ResourceModel>.Fail("资源不存在");
            }
            var res = model.CopyTo<ResourceModel>();
            res.Tags = Tag().GetTags(id);
            res.Files = db.ResourceFiles.Where(i => i.ResId == id).ToArray();
            res.MetaItems = new MetaRepository(db).GetOrDefault(id);
            return OperationResult.Ok(res);
        }

        public IOperationResult<ResourceModel> GetFull(int id)
        {
            var model = db.Resources.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<ResourceModel>.Fail("资源不存在");
            }
            model.ViewCount++;
            db.SaveChanges();
            var res = model.CopyTo<ResourceModel>();
            res.Content = Markdig.Markdown.ToHtml(model.Content);
            res.User = userStore.Get(model.UserId);
            res.Category = db.Categories.Where(i => i.Id == model.CatId)
                .Select(i => new ListLabelItem(i.Id, i.Name)).SingleOrDefault();
            res.IsGradable = IsGradable(id);
            res.Tags = Tag().GetTags(id);
            res.Files = db.ResourceFiles.Where(i => i.ResId == id).ToArray();
            res.MetaItems = new MetaRepository(db).GetOrDefault(id);
            if (res.MetaItems.TryGetValue("file_catalog", out var text))
            {
                res.FileCatalog = JsonSerializer.Deserialize<CatalogItem[]>(text);
                res.MetaItems.Remove("file_catalog");
            }
            return OperationResult.Ok(res);
        }

        public OperationResult<ResourceModel> GetPreview(int id)
        {
            var model = db.Resources.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<ResourceModel>.Fail("资源不存在");
            }
            var res = model.CopyTo<ResourceModel>();
            // data = array_merge(data, ResourceMetaModel.GetOrDefault(id));
            res.PreviewUrl = $"./preview/view/0/id/{id}/file/0/";
            return OperationResult.Ok(res);
        }

        public bool IsGradable(int id)
        {
            if (!Log().Has(LOG_TYPE_RES, id,
                LOG_ACTION_DOWNLOAD))
            {
                return false;
            }
            return !Score().Has(id);
        }

        public IOperationResult<ScoreModel> GradeScore(int id, byte score)
        {
            if (score < 1 || score > 10)
            {
                return OperationResult<ScoreModel>.Fail("分数不正确");
            }
            if (!Log().Has(LOG_TYPE_RES, id,
                LOG_ACTION_DOWNLOAD))
            {
                return OperationResult<ScoreModel>.Fail("请先使用后再来评价");
            }
            var provider = Score();
            if (provider.Has(id))
            {
                return OperationResult<ScoreModel>.Fail("已评价过，不能更改");
            }
            var data = provider.Add(score, id);

            var avg = provider.Avg(id);
            db.Resources.Where(i => i.Id == id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Score, avg));
            var res = data.CopyTo<ScoreModel>();
            res.Avg = avg;
            return OperationResult.Ok(res);
        }

        public IOperationResult<ResourceEntity> Save(ResourceForm data, 
            string[] tags, ResourceFileForm[] files)
        {
            var model = data.Id > 0 ? db.Resources.Where(i => i.Id == data.Id
                && i.UserId == client.UserId).FirstOrDefault()
                : new ResourceModel();
            if (model is null)
            {
                return OperationResult<ResourceEntity>.Fail("数据有误");
            }
            model.Thumb = data.Thumb;

            model.UserId = client.UserId;
            db.Resources.Save(model, client.Now);
            db.SaveChanges();
            Tag().BindTag(model.Id, tags, null);
            var fileId = new List<int>();
            foreach (var item in files)
            {
                var fileModel = FileSave(new FileForm()
                {
                    Id = item.Id,
                    ResId = model.Id,
                    File = item.File,
                    FileType = item.FileType,
                });
                if (!fileModel.Succeeded)
                {
                    continue;
                }
                fileId.Add(fileModel.Result.Id);
                if (fileModel.Result.FileType > 0)
                {
                    continue;
                }
                //self.Storage().AddQuote(fileModel.File, Constants.TYPE_RESOURCE_STORE, model.Id);
                //file = UploadRepository.File(fileModel);
                //if (file.Exist())
                //{
                //    model.Size = file.Size();
                //}
                //data["file_catalog"] = UploadRepository.Catalog(fileModel);
            }
            db.ResourceFiles.Where(i => i.ResId == model.Id && fileId.Contains(i.Id))
                .ExecuteDelete();
            //new MetaRepository(db).SaveBatch(model.Id, data);
            return OperationResult.Ok(model);
        }

        public IOperationResult Remove(int id)
        {
            var model = db.Resources.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("id is error");
            }
            return RemoveResource(model);
        }

        private IOperationResult RemoveResource(ResourceEntity? model)
        {
            if (model is null)
            {
                return OperationResult.Fail("资源不存在");
            }
            var items = db.ResourceFiles.Where(i => i.ResId == model.Id).ToArray();
            foreach (var item in items)
            {
                RemoveFile(item);
            }
            db.Resources.Remove(model);
            db.SaveChanges();
            return OperationResult.Ok();
        }

        private void RemoveFile(ResourceFileEntity model)
        {
            if (model.FileType > 0)
            {
                db.ResourceFiles.Remove(model);
                return;
            }
            //file = UploadRepository.File(model);
            //if (file.Exist())
            //{
            //    file.Delete();
            //}
            //folder = UploadRepository.ResourceFolder(model.Id);
            //if (folder.Exist())
            //{
            //    folder.Delete();
            //}
            //model.Delete();
        }

        public CatalogItem[] GetCatalog(int id)
        {
            var text = db.Metas.Where(i => i.ItemId == id && i.Name == "file_catalog")
                .Select(i => i.Content).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(text))
            {
                return [];
            }
            return JsonSerializer.Deserialize<CatalogItem[]>(text);
        }

        public ListArticleItem[] Suggestion(string keywords)
        {
            return db.Resources.Search(keywords, "title")
                .Take(4).Select(i => new ListArticleItem(i.Id, i.Title))
                .ToArray();
        }

        public IPage<ResourceListItem> GetManageList(QueryForm form, 
            int user = 0, int category = 0)
        {
            var res = AsList(db.Resources
                .When(category > 0, i => i.CatId == category)
                .When(user > 0, i => i.UserId == user).Search(form.Keywords, "title")
                .OrderByDescending(i => i.Id)).ToPage(form);
            userStore.Include(res.Items);
            CategoryRepository.Include(db, res.Items);
            return res;
        }

        public IOperationResult<ResourceEntity> GetSelf(int id)
        {
            var model = db.Resources.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<ResourceEntity>.Fail("资源不存在");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult RemoveSelf(int id)
        {
            return RemoveResource(db.Resources.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault());
        }

        public IPage<ResourceFileEntity> FileList(int res_id, QueryForm form)
        {
            return db.ResourceFiles.Where(i => i.ResId == res_id)
                .OrderBy(i => i.Id)
                .ToPage(form);
        }

        public IOperationResult<ResourceFileEntity> FileSave(FileForm data)
        {
            var model = data.Id > 0 ?
                db.ResourceFiles.Where(i => i.Id == data.Id).SingleOrDefault()
                : new ResourceFileEntity();
            if (model is null)
            {
                return OperationResult.Fail<ResourceFileEntity>("id is error");
            }
            model.ResId = data.ResId;
            model.File = data.File;
            model.FileType = data.FileType;
            model.UserId = client.UserId;
            db.ResourceFiles.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult FileRemove(int id)
        {
            var model = db.ResourceFiles.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<ResourceEntity>.Fail("文件不存在");
            }
            RemoveFile(model);
            return OperationResult.Ok();
        }

        public IOperationResult<FileResult> Download(int id, int file = 0)
        {
            var model = db.Resources.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<FileResult>.Fail("资源不存在");
            }
            Log().Insert(new()
            {
                ItemType = LOG_TYPE_RES,
                ItemId = id,
                Action = LOG_ACTION_DOWNLOAD
            });

            var fileModel = file > 0 ?
                db.ResourceFiles.Where(i => i.ResId == id && i.Id == file).FirstOrDefault() :
                db.ResourceFiles.Where(i => i.ResId == id && i.FileType == 0).OrderBy(i => i.FileType)
                .FirstOrDefault();
            if (fileModel is null)
            {
                return OperationResult<FileResult>.Fail("没有可下载的文件");
            }
            model.DownloadCount++;
            db.Resources.Save(model);
            db.SaveChanges();
            return UploadRepository.File(fileModel);
        }


        public static IQueryable<ResourceListItem> AsList(IQueryable<ResourceEntity> source)
        {
            return source.Select(i => new ResourceListItem()
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Keywords = i.Keywords,
                Thumb = i.Thumb,
                Size = i.Size,
                Score = i.Score,
                UserId = i.UserId,
                PreviewType = i.PreviewType,
                CatId = i.CatId,
                Price = i.Price,
                IsCommercial = i.IsCommercial,
                IsReprint = i.IsReprint,
                CommentCount = i.CommentCount,
                DownloadCount = i.DownloadCount,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
                ViewCount = i.ViewCount,
            });
        }
    }
}
