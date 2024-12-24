using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OpenPlatform.Entities;
using NetDream.Modules.OpenPlatform.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.OpenPlatform.Repositories
{
    public class PlatformRepository(OpenContext db, IClientContext client)
    {
        public const byte STATUS_NONE = 0;
        public const byte STATUS_WAITING = 9;
        public const byte STATUS_SUCCESS = 1;

        public IPage<PlatformEntity> GetList(string keywords = "", int page = 1)
        {
            return db.Platforms.Search(keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(page);
        }

        public PlatformEntity? Get(int id)
        {
            return db.Platforms.Where(i => i.Id == id).Single();
        }
        public IOperationResult<PlatformEntity> Save(PlatformForm data)
        {
            var model = data.Id > 0 ? db.Platforms.Where(i => i.Id == data.Id)
                .Single() :
                new PlatformEntity();
            if (model is null)
            {
                return OperationResult.Fail<PlatformEntity>("id error");
            }
            if (model.UserId == 0)
            {
                model.UserId = client.UserId;
            }
            model.Name = data.Name;
            model.Description = data.Description;
            model.Type = data.Type;
            model.Domain = data.Domain;
            model.SignKey = data.SignKey;
            model.SignType = data.SignType;
            model.EncryptType = data.EncryptType;
            model.PublicKey = data.PublicKey;
            model.AllowSelf = data.AllowSelf;
            db.Platforms.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Platforms.Where(i => i.Id == id).ExecuteDelete();
        }
    }
}
