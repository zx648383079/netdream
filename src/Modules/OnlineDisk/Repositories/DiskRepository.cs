using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.OnlineDisk.Repositories
{
    public class DiskRepository
    {
        public const int SHARE_PUBLIC = 0; //公开分享
        public const int SHARE_PROTECTED = 1; //密码分享
        public const int SHARE_PRIVATE = 2;  //分享给个人
        public bool UseDistributed => false;
    }
}
