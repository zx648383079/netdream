using NetDream.Web.Areas.Open.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Open.Repositories
{
    public class OpenRepository
    {
        private readonly IDatabase _db;

        public OpenRepository(IDatabase db)
        {
            _db = db;
        }

        public PlatformModel GetByAppId(string appid)
        {
            return _db.Single<PlatformModel>("where appid=@0", appid);
        }
    }
}
