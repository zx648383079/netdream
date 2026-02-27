using NetDream.Modules.Finance.Entities;
using NetDream.Shared.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Finance.Importers
{
    public class WxV2Importer(FinanceContext db, IClientContext client) : IImporter<LogEntity>
    {
        private int _accountId;

        private void Ready()
        {
            if (_accountId == 0)
            {
                _accountId = db.Account.Where(i => i.UserId == client.UserId && i.Name == "微信").Select(i => i.Id)
                    .FirstOrDefault();
            }
        }

        public bool IsMatch(Stream input, string fileName)
        {
            return fileName.EndsWith(".xlsx") && fileName.StartsWith("微信");
        }

        public IEnumerable<LogEntity> Read(Stream input)
        {
            input.Seek(0, SeekOrigin.Begin);
            Ready();
            using var package = new ExcelPackage(input);
            var worksheet = package.Workbook.Worksheets[0];
            var columns = new string[worksheet.Cells.Columns];
            var headingRow = 17;
            for (var col = 0; col < columns.Length; col++)
            {
                columns[col] = worksheet.Cells[headingRow, col].GetCellValue<string>();
            }
            for (var row = headingRow + 1; row < worksheet.Cells.Rows; row++)
            {
                var item = FormatData(worksheet.Cells, row, columns);
                if (item != null)
                {
                    yield return item;
                }
            }
        }

        private LogEntity? FormatData(ExcelRange cells, int row, string[] columns)
        {
            var type = GetCellValue <string>(cells, row, columns, "收/支");
            if (string.IsNullOrWhiteSpace(type) || type.Trim() == "/")
            {
                return null;
            }
            return new LogEntity()
            {
                Type = (byte)(type == "支出" ? 0 : 1),
                Money = decimal.Parse(Regex.Match(GetValue(columns, data, "金额(元)"), @"[^\d\.]+").Value),
                AccountId = _accountId,
                TradingObject = GetCellValue<string>(cells, row, columns, "交易对方"),
                Remark = GetCellValue<string>(cells, row, columns, "商品"),
                UserId = client.UserId,
                OutTradeNo = string.Format("wx{0}",
                    GetCellValue<string>(cells, row, columns, "交易单号")),
                CreatedAt = client.Now,
                UpdatedAt = client.Now,
                HappenedAt = GetCellValue<DateTime>(cells, row, columns, "交易时间")
            };
        }

        private T? GetCellValue<T>(ExcelRange cells, int row, string[] columns, string find)
        {
            var i = columns.IndexOf(find);
            if (i < 0)
            {
                return default;
            }
            return cells[row, i].GetCellValue<T>();
        }

        public void Dispose()
        {
        }
    }
}
