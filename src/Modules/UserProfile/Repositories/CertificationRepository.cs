using Microsoft.EntityFrameworkCore;
using NetDream.Modules.UserProfile.Entities;
using NetDream.Modules.UserProfile.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.UserProfile.Repositories
{
    public class CertificationRepository(ProfileContext db, 
        IClientContext client)
    {
        public IPage<CertificationEntity> GetMangeList(QueryForm form)
        {
            return db.Certifications.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public CertificationEntity Get(int id)
        {
            var model = db.Certifications.Where(i => i.Id == id && i.UserId == client.UserId)
                .SingleOrDefault();
            model ??= new CertificationEntity();
            return model;
        }
        public IOperationResult<CertificationEntity> Save(CertificationForm data)
        {
            var model = data.Id > 0 ? db.Certifications
                .Where(i => i.Id == data.Id && i.UserId == client.UserId)
                .SingleOrDefault() :
                new CertificationEntity();
            if (model is null)
            {
                return OperationResult.Fail<CertificationEntity>("id error");
            }
            model.Name = data.Name;
            model.Profession = data.Profession;
            model.FrontSide = data.FrontSide;
            model.BackSide = data.BackSide;
            model.Country = data.Country;
            model.Sex = data.Sex;
            model.CardNo = data.CardNo;
            model.Type = data.Type;
            model.ExpiryDate = data.ExpiryDate;
            model.UserId = client.UserId;
            db.Certifications.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Certifications.Where(i => i.Id == id && i.UserId == client.UserId)
                .ExecuteDelete();
            db.SaveChanges();
        }
    }
}
