using Modules.AdSense.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.AdSense.Repositories
{
    public class AdRepository(IDatabase db)
    {
        public const int TYPE_TEXT = 0;
        public const int TYPE_IMAGE = 1;
        public const int TYPE_HTML = 2;
        public const int TYPE_VIDEO = 3;

        public void ManagePositionRemove(int id)
        {
            db.Delete<PositionEntity>(id);
            db.Delete<AdEntity>("WHERE position_id=@0", id);
        }

        public IList<PositionEntity> PositionAll()
        {
            return db.Fetch<PositionEntity>();
        }
    }
}
