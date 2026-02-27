using NetDream.Modules.Finance.Entities;
using NetDream.Shared.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Finance.Importers
{
    public class WxImporter(FinanceContext db, IClientContext client) : CsvImporter
    {
        private int _accountId;

        protected override void Ready()
        {
            if (_accountId == 0)
            {
                _accountId = db.Account.Where(i => i.UserId == client.UserId && i.Name == "微信").Select(i => i.Id)
                    .FirstOrDefault();
            }
        }

        public override bool IsMatch(Stream input, string fileName)
        {
            return fileName.EndsWith(".csv") &&  fileName.StartsWith("微信");
        }

        protected override LogEntity? FormatData(string[] columns, string[] data)
        {
            return new LogEntity()
            {
                Type = (byte)(GetValue(columns, data, "收/支") == "支出" ? 0 : 1),
                Money = decimal.Parse(Regex.Match(GetValue(columns, data, "金额(元)"), @"[^\d\.]+").Value),
                AccountId = _accountId,
                Remark = string.Format("{0} {1}", 
                    GetValue(columns, data, "交易对方"), GetValue(columns, data, "商品")),
                UserId = client.UserId,
                OutTradeNo = string.Format("wx{0}",
                    GetValue(columns, data, "交易单号")),
                CreatedAt = client.Now,
                UpdatedAt = client.Now,
                HappenedAt = DateTime.Parse(GetValue(columns, data, "交易时间"))
            };
        }
    }
}
