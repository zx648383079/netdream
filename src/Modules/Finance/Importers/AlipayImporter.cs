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
            return fileName.EndsWith(".csv") && fileName.StartsWith("支付宝");
        }

        protected override LogEntity? FormatData(string[] columns, string[] data)
        {
            if (GetValue(columns, data, "交易状态") != "交易成功") { // '交易成功' '已关闭' '冻结成功'
                return null;
            }
            var time = GetValue(columns, data, "付款时间");
            if (string.IsNullOrEmpty(time))
            {
                time = GetValue(columns, data, "交易时间");
            }
            return new LogEntity()
            {
                Type = (byte)(GetValue(columns, data, "收/支") == "支出" ? 0 : 1), // '不计收支' '支出' '收入'
                Money = decimal.Parse(Regex.Match(GetValue(columns, data, "金额"), @"[^\d\.]+").Value),
                AccountId = _accountId,
                TradingObject = GetValue(columns, data, "交易对方"),
                Remark = GetValue(columns, data, "商品说明"),
                UserId = client.UserId,
                OutTradeNo = string.Format("ali{0}",
                    GetValue(columns, data, "交易订单号")),
                CreatedAt = client.Now,
                UpdatedAt = client.Now,
                HappenedAt = DateTime.Parse(time)
            };
        }
    }
}
