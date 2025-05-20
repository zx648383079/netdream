using NetDream.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Finance.Repositories
{
    public class LogRepository(FinanceContext db, IClientContext client)
    {
        /// <summary>
        /// 支出
        /// </summary>
        public const byte TYPE_EXPENDITURE = 0;
        /// <summary>
        /// 收入
        /// </summary>
        public const byte TYPE_INCOME = 1;

        public const byte TYPE_LEND = 2; // 借出
        public const byte TYPE_BORROW = 3; // 借入
    }
}
