using NetDream.Shared.Helpers;
using NetDream.Modules.SEO.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDream.Shared.Repositories;
using NetDream.Shared.Migrations;
using NetDream.Modules.SEO.Models;
using NetDream.Shared.Extensions;

namespace NetDream.Modules.SEO.Repositories
{
    public class AgreementRepository(IDatabase db, LocalizeRepository localize) : CRUDRepository<AgreementEntity>(db)
    {

        public override IList<string> SearchKeys => ["name", "title"];


        public AgreementEntity Detail(int id)
        {
            var model = db.SingleById<AgreementModel>(id);
            var sql = new Sql();
            sql.Select("id", LocalizeRepository.LANGUAGE_COLUMN_KEY)
                .From<AgreementEntity>(db)
                .Where("name=@0 AND status=@1", model.Name, model.Status)
                .OrderBy("created_at ASC");
            var items = db.Fetch<AgreementEntity>(sql);
            model.Languages = localize.FormatLanguageList(items, false);
            return model;
        }

        public AgreementEntity GetByName(string name)
        {
            var model = localize.GetByKey<AgreementEntity>(
                db,
                new Sql().Select("*").From<AgreementEntity>(db)
                .Where("status=1"),
                "name",
                name
            );
            if (model is null)
            {
                throw new Exception("Service agreement does not exist");
            }
            return model;
        }

        protected override void AfterSave(int id, AgreementEntity data)
        {
            if (data.Status > 0)
            {
                db.Update<AgreementEntity>("SET status=0 WHERE name=@0 AND language=@1 AND id<>@2", 
                    data.Name, data.Language, data.Id);
            }
        }

    }
}
