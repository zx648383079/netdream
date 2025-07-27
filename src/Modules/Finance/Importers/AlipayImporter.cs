using NetDream.Modules.Finance.Entities;
using NetDream.Shared.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Finance.Importers
{
    public class AlipayImporter(FinanceContext db, IClientContext client) : CsvImporter
    {
        private int _accountId;

        private readonly Encoding _encoding = Encoding.GetEncoding("gbk");
        public override Encoding Encoding => _encoding;

        protected override void Ready()
        {
            if (_accountId == 0)
            {
                _accountId = db.Account.Where(i => i.UserId == client.UserId 
                && i.Name == "支付宝").Select(i => i.Id)
                    .FirstOrDefault();
            }
        }

        public override bool IsMatch(Stream input, string fileName)
        {
            return FirstRowContains(input, "支付宝");
        }

        protected override LogEntity? FormatData(string[] columns, string[] data)
        {
            var time = GetValue(columns, data, "付款时间");
            if (string.IsNullOrEmpty(time))
            {
                time = GetValue(columns, data, "交易创建时间");
            }
            return new LogEntity()
            {
                Type = (byte)(GetValue(columns, data, "收/支") == "支出" ? 0 : 1),
                Money = decimal.Parse(Regex.Match(GetValue(columns, data, "金额（元）"), @"[^\d\.]+").Value),
                AccountId = _accountId,
                Remark = string.Format("{0} {1}",
                    GetValue(columns, data, "交易对方"), GetValue(columns, data, "商品名称")),
                UserId = client.UserId,
                OutTradeNo = string.Format("ali{0}",
                    GetValue(columns, data, "交易号")),
                CreatedAt = client.Now,
                UpdatedAt = client.Now,
                HappenedAt = DateTime.Parse(time)
            };
        }
    }
}
