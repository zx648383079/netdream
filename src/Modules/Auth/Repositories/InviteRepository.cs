using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Repositories
{
    public class InviteRepository
    {
        public const int TYPE_CODE = 0; // 邀请码
        public const int TYPE_LOGIN = 5; // 扫码登录

        public const int STATUS_UN_SCAN = 0;  //未扫码
        public const int STATUS_UN_CONFIRM = 1;  // 已扫码待确认
        public const int STATUS_SUCCESS = 2;     // 登录成功
        public const int STATUS_REJECT = 3;      // 拒绝登录
    }
}
