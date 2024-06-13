using NetDream.Core.Helpers;
using NetDream.Modules.SEO.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.SEO.Repositories
{
    public class AgreementRepository(IDatabase db)
    {
        public Page<AgreementEntity> GetList(string keywords = "", long page = 1)
        {
            var sql = new Sql();
            SearchHelper.Where(sql, ["name", "title"], keywords);
            sql.OrderBy("created_at DESC");
            return db.Page<AgreementEntity>(page, 20, sql);
        }

        public AgreementEntity Get(int id)
        {
            return db.SingleById<AgreementEntity>(id);
        }

        
    }
}
