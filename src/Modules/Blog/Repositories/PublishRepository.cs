using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Blog.Repositories
{
    public class PublishRepository
    {
        public const int PUBLISH_STATUS_DRAFT = 0; // 草稿
        public const int PUBLISH_STATUS_POSTED = 5; // 已发布
        public const int PUBLISH_STATUS_TRASH = 9; // 垃圾箱
        public const int PUBLISH_STATUS_AUTO_SAVE = 8; // 自动保存

        public const int TYPE_ORIGINAL = 0; // 原创
        public const int TYPE_REPRINT = 1; // 转载

        public const int EDIT_HTML = 0;
        public const int EDIT_MARKDOWN = 1; // markdown

        public const int OPEN_PUBLIC = 0; // 公开
        public const int OPEN_LOGIN = 1; // 需要登录
        public const int OPEN_PASSWORD = 5; // 需要密码
        public const int OPEN_BUY = 6; // 需要购买
    }
}
