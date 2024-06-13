using NetDream.Modules.OpenPlatform.Entities;
using NetDream.Modules.OpenPlatform.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.OpenPlatform.Repositories
{
    public class OpenRepository(IDatabase db)
    {
        public PlatformModel GetByAppId(string appId)
        {
            return db.Single<PlatformModel>("where appid=@0", appId);
        }
    }
}
