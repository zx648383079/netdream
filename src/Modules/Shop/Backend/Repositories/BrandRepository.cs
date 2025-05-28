using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Backend.Forms;
using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    public class BrandRepository(ShopContext db, IClientContext client)
    {
        public IPage<BrandEntity> GetList(QueryForm form)
        {
            return db.Brands.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public IOperationResult<BrandEntity> Get(int id)
        {
            var model = db.Brands.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<BrandEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<BrandEntity> Save(BrandForm data)
        {
            var model = data.Id > 0 ? db.Brands.Where(i => i.Id == data.Id).SingleOrDefault()
                : new BrandEntity();
            if (model == null)
            {
                return OperationResult.Fail<BrandEntity>("数据有误");
            }
            model.Name = data.Name;
            model.Description = data.Description;
            model.Keywords = data.Keywords;
            model.Logo = data.Logo;
            model.AppLogo = data.AppLogo;
            model.Url = data.Url;
            db.Brands.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Brands.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public void Refresh()
        {
            db.Brands.RefreshPk((old_id, new_id) => {
                db.Goods.Where(i => i.BrandId == old_id)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.BrandId, new_id));
            });
            db.SaveChanges();
        }

        public ListLabelItem[] All()
        {
            return db.Brands
                .OrderBy(i => i.Id)
                .Select(i => new ListLabelItem(i.Id, i.Name)).ToArray();
        }

        public IPage<BrandEntity> Search(QueryForm form, int[] idItems)
        {
            return db.Brands.Search(form.Keywords, "name")
                .When(idItems.Length > 0, i => idItems.Contains(i.Id))
                .OrderBy(i => i.Id)
                .ToPage(form);
        }

        public int FindOrNew(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return 0;
            }
            var id = db.Brands.Where(i => i.Name == name).Value(i => i.Id);
            if (id > 0)
            {
                return id;
            }
            var model = new BrandEntity()
            {
                Name = name,
            };
            db.Brands.Add(model);
            db.SaveChanges();
            return model.Id;
        }
    }
}
