using Microsoft.EntityFrameworkCore;
using NetDream.Modules.SEO.Entities;
using NetDream.Modules.SEO.Forms;
using NetDream.Modules.SEO.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using NetDream.Shared.Repositories.Models;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.SEO.Repositories
{
    public class AgreementRepository(
        SEOContext db, 
        IClientContext client,
        ILocalizeRepository localize)
    {

        public IPage<AgreementListItem> GetList(QueryForm form)
        {
            return db.Agreements.Search(form.Keywords, "name", "title")
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
        }

        public AgreementEntity? Get(int id)
        {
            return db.Agreements.Where(i => i.Id == id).Single();
        }
        public IOperationResult<AgreementEntity> Save(AgreementForm data)
        {
            var model = data.Id > 0 ? db.Agreements.Where(i => i.Id == data.Id).Single() :
                new AgreementEntity();
            if (model is null)
            {
                return OperationResult.Fail<AgreementEntity>("id error");
            }
            model.Name = data.Name;
            model.Title = data.Title;
            model.Language = data.Language;
            model.Description = data.Description;
            model.Content = data.Content;
            model.Status = data.Status;
            if (model.CreatedAt == 0)
            {
                model.CreatedAt = client.Now;
            }
            model.UpdatedAt = client.Now;
            db.Agreements.Save(model);
            db.SaveChanges();
            AfterSave(model.Id, model);
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Agreements.Where(i => i.Id == id).ExecuteDelete();
        }

        public IOperationResult<AgreementEntity> Detail(int id)
        {
            var model = db.Agreements.Where(i => i.Id == id).Single();
            if (model is null)
            {
                return OperationResult.Fail<AgreementEntity>("id error");
            }
            var items = db.Agreements.Where(i => i.Name == model.Name && i.Status == model.Status)
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => new KeyValuePair<string, int>(i.Language, i.Id))
                .ToDictionary();
            var Languages = localize.Items.Select(i => {
                var item = (LanguageFormatted)i;
                if (items.TryGetValue(item.Value, out var id))
                {
                    item.Id = id;
                }
                return item;
            }).ToArray();
            return OperationResult.Ok(model);
        }

        public IOperationResult<AgreementModel> GetByName(string name)
        {
            var model = db.Agreements
                .Where(i => i.Status == (byte)ReviewStatus.Approved && i.Name == name && i.Language == localize.Language)
                .SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<AgreementModel>("Service agreement does not exist");
            }
            return OperationResult.Ok(new AgreementModel(model));
        }

        protected void AfterSave(int id, AgreementEntity data)
        {
            if (data.Status > 0)
            {
                db.Agreements.Where(i => i.Name == data.Name && i.Language == data.Language && i.Id != data.Id)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, 0));
            }
        }

    }
}
