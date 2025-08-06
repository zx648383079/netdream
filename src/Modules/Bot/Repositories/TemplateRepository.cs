using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Bot.Entities;
using NetDream.Modules.Bot.Forms;
using NetDream.Modules.Bot.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Bot.Repositories
{
    public class TemplateRepository(BotContext db, IClientContext client)
    {
        public static Dictionary<int, string> TypeItems = new()
        {
            { 0, "标题样式" },
            { 1, "正文样式"},
            { 2, "图文样式"},
            { 3, "引导样式"},
            { 4, "分割线"},
            { 5, "二维码"},
            { 6, "音频样式"},
            { 7, "视频样式"},
            { 8, "图标样式"},

            { 91, "节日模板"},
            { 92, "行业模板"}
        };
        public IPage<EditorTemplateEntity> GetList(TemplateQueryForm form)
        {
            return db.EditorTemplates.Search(form.Keywords, "name")
                .When(form.Type >= 0, i => i.Type == form.Type)
                .When(form.Category > 0, i => i.CatId == form.Category)
                .OrderByDescending(i => i.Id).ToPage(form);
        }

        public IOperationResult<EditorTemplateEntity> Get(int id)
        {
            var model = db.EditorTemplates.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<EditorTemplateEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.EditorTemplates.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public IOperationResult<EditorTemplateEntity> Save(ETemplateForm data)
        {
            var model = data.Id > 0 ? db.EditorTemplates.Where(i => i.Id == data.Id)
                .SingleOrDefault() :
                new EditorTemplateEntity();
            if (model is null)
            {
                return OperationResult.Fail<EditorTemplateEntity>("id error");
            }
            model.Name = data.Name;
            model.Content = data.Content;
            model.Type = data.Type;
            model.CatId = data.CatId;
            db.EditorTemplates.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IPage<EditorCategoryEntity> CategoryList(QueryForm form)
        {
            return db.EditorCategories.Search(form.Keywords, "name")
                .OrderByDescending(i => i.ParentId)
                .ToPage(form);
        }

        public IOperationResult<EditorCategoryEntity> Category(int id)
        {
            var model = db.EditorCategories.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<EditorCategoryEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<EditorCategoryEntity> CategorySave(CategoryForm data)
        {
            var model = data.Id > 0 ? db.EditorCategories.Where(i => i.Id == data.Id)
                .SingleOrDefault() :
                new EditorCategoryEntity();
            if (model is null)
            {
                return OperationResult.Fail<EditorCategoryEntity>("id error");
            }
            model.Name = data.Name;
            model.ParentId = data.ParentId;
            db.EditorCategories.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void CategoryRemove(int id)
        {
            db.EditorCategories.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public OptionItem<int>[] TypeList()
        {
            return TypeItems.Select(i => new OptionItem<int>(i.Value, i.Key)).ToArray();
        }

        public CategoryTreeItem[] CategoryAll()
        {
            var items = db.EditorCategories.Select(i => new CategoryTreeItem()
            {
                Id = i.Id,
                Name = i.Name,
                ParentId = i.ParentId,
            }).ToArray();
            return TreeHelper.Sort(items);
        }
    }
}
