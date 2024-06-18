using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.CMS.Repositories
{
    public class SiteRepository
    {
        public const int TYPE_CATEGORY = 0;
        public const int TYPE_ARTICLE = 1;
        public const int TYPE_COMMENT = 2;
        public const int TYPE_MODEL = 3;
        public const int TYPE_MODEL_FIELD = 4;
        public const int TYPE_LINKAGE = 5;
        public const int TYPE_LINKAGE_DATA = 6;
        public const int TYPE_GROUP = 7;
        public const int TYPE_SITE = 8;

        public const int ACTION_AGREE = 1;
        public const int ACTION_DISAGREE = 2;

        public const int PUBLISH_STATUS_DRAFT = 0; // 草稿
        public const int PUBLISH_STATUS_POSTED = 5; // 已发布
        public const int PUBLISH_STATUS_TRASH = 9; // 垃圾箱
    }
}
