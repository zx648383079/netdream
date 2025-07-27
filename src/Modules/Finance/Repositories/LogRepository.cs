using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Finance.Entities;
using NetDream.Modules.Finance.Forms;
using NetDream.Modules.Finance.Importers;
using NetDream.Modules.Finance.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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


        public IPage<LogListItem> GetList(LogQueryForm form)
        {
            return BindQuery(db.Log.Where(i => i.UserId == client.UserId), form.Type, 
                form.Keywords, form.Account, form.Budget, 
                DateTime.TryParse(form.StartAt, out var s) ? s : null ,
                DateTime.TryParse(form.EndAt, out var e) ? e : null)
                .OrderByDescending(i => i.HappenedAt).ToPage(form).CopyTo<LogEntity, LogListItem>();
        }

        public int Count(LogQueryForm form)
        {
            return BindQuery(db.Log.Where(i => i.UserId == client.UserId), 
                form.Type, form.Keywords, form.Account, form.Budget,
                DateTime.TryParse(form.StartAt, out var s) ? s : null,
                DateTime.TryParse(form.EndAt, out var e) ? e : null).Count();
        }

        private static IQueryable<LogEntity> BindQuery(IQueryable<LogEntity> builder, 
            int type = 0, string keywords = "", 
            int account = 0, int budget = 0, DateTime? start_at = null, DateTime? end_at = null)
        {
            return builder.When(type > 0, i => i.Type == type - 1)
                .Search(keywords, "remark")
                .When(account > 0, i => i.AccountId == account)
                .When(budget > 0, i => i.BudgetId == budget)
                .When(start_at is not null, i => i.HappenedAt >= start_at)
                .When(end_at is not null, i => i.HappenedAt <= end_at);
        }

        /**
         * 获取
         * @param int id
         * @return LogModel
         * @throws Exception
         */
        public IOperationResult<LogEntity> Get(int id)
        {
            var model = db.Log.Where(i => i.UserId == client.UserId && i.Id == id)
                .FirstOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<LogEntity>("记录不存在");
            }
            return OperationResult.Ok(model);
        }

        /**
         * 保存
         * @param array data
         * @return LogModel
         * @throws Exception
         */
        public IOperationResult<LogEntity> Save(LogForm data)
        {
            LogEntity? model;
            if (data.Id > 0)
            {
                model = db.Log.Where(i => i.UserId == client.UserId && i.Id == data.Id)
                .FirstOrDefault();
            }
            else
            {
                model = new();
            }
            if (model is null)
            {
                return OperationResult.Fail<LogEntity>("记录不存在");
            }
            model.Type = data.Type;
            model.TradingObject = data.TradingObject;
            model.Remark = data.Remark;
            model.OutTradeNo = data.OutTradeNo;
            model.AccountId = data.AccountId;
            model.BudgetId = data.BudgetId;
            model.ChannelId = data.ChannelId;
            model.Money = data.Money;
            model.FrozenMoney = data.FrozenMoney;
            model.HappenedAt = data.HappenedAt;
            model.UserId = client.UserId;
            db.Log.Save(model, client.Now);
            db.SaveChanges();
            if (model.BudgetId > 0)
            {
                new BudgetRepository(db, client)
                    .RefreshSpent(model.BudgetId);
            }
            return OperationResult.Ok(model);
        }

        /**
         * 删除产品
         * @param int id
         * @return mixed
         */
        public void Remove(int id)
        {
            db.Log.Where(i => i.UserId == client.UserId && i.Id == id)
               .ExecuteDelete();
            db.SaveChanges();
        }

        public int BatchEdit(BatchLogForm form)
        {
            if (string.IsNullOrWhiteSpace(form.Keywords))
            {
                return 0;
            }
            var data = new Dictionary<string, int>();
            if (form.AccountId > 0)
            {
                data.Add("account_id", form.AccountId);
            }
            if (form.ProjectId > 0)
            {
                data.Add("project_id", form.ProjectId);
            }
            if (form.ChannelId > 0)
            {
                data.Add("channel_id", form.ChannelId);
            }
            if (form.BudgetId > 0)
            {
                data.Add("budget_id", form.BudgetId);
            }
            if (data.Count == 0)
            {
                return 0;
            }
            var query = db.Log.Where(i => i.UserId == client.UserId)
                .Search(form.Keywords, "remark");

            // 构建 SET 子句
            var parameters = new List<object>();
            var setClauses = new List<string>();
            int paramIndex = 0;

            foreach (var prop in data)
            {
                setClauses.Add($"{prop.Key} = {{{paramIndex}}}");
                parameters.Add(prop.Value);
                paramIndex++;
            }

            string setClause = string.Join(", ", setClauses);

            // 执行原始 SQL
            return db.Database.ExecuteSqlRaw(
                $"UPDATE {db.Model.FindEntityType(typeof(LogEntity)).GetTableName()} " +
                $"SET {setClause} " +
                $"WHERE {query.ToQueryString().Split("WHERE")[1]}",
                parameters.ToArray());
        }


        public IOperationResult SaveDay(DayLogForm form)
        {
            var items = new LogPartialForm[] { form.Breakfast, form.Lunch, form.Dinner };
            foreach (var item in items)
            {
                if (item is null || item.Money <= 0)
                {
                    continue;
                }
                db.Log.Add(new LogEntity()
                {
                    Type = TYPE_EXPENDITURE,
                    Money = item.Money,
                    AccountId = form.AccountId,
                    ChannelId = form.ChannelId,
                    BudgetId = form.BudgetId,
                    Remark = item.Remark,
                    TradingObject = item.TradingObject,
                    UserId = client.UserId,
                    CreatedAt = client.Now,
                    UpdatedAt = client.Now,
                    HappenedAt = DateTime.Parse($"{form.Day} {item.Time}")
                });
            }
            db.SaveChanges();
            return OperationResult.Ok();
        }
        public IOperationResult Import(Stream input)
        {
            var items = new IImporter<LogEntity>[]
            {
                new WxImporter(db, client),
                new AlipayImporter(db, client),
            };
            foreach (var importer in items)
            {
                if (!importer.IsMatch(input, string.Empty))
                {
                    continue;
                }
                foreach (var item in importer.Read(input))
                {
                    db.Log.Add(item);
                }
                db.SaveChanges();
                return OperationResult.Ok();
            }
            return OperationResult.Fail("不支持");
        }

        public void Export(Stream output)
        {
            //var title = "流水记录";
            //if (urlEncode)
            //{
            //    title = Uri.EscapeDataString(title);
            //}
            ExcelPackage.License.SetNonCommercialPersonal("zodream");
            using (var package = new ExcelPackage())
            {
                // 添加工作表
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // 添加数据
                worksheet.Cells["A1"].Value = "ID";
                worksheet.Cells["B1"].Value = "类型";
                worksheet.Cells["C1"].Value = "金额";
                worksheet.Cells["D1"].Value = "冻结金额";
                worksheet.Cells["E1"].Value = "账户";
                worksheet.Cells["F1"].Value = "渠道";
                worksheet.Cells["G1"].Value = "项目";
                worksheet.Cells["H1"].Value = "预算";
                worksheet.Cells["I1"].Value = "备注";
                worksheet.Cells["J1"].Value = "交易单号";
                worksheet.Cells["K1"].Value = "记录时间";
                worksheet.Cells["L1"].Value = "更新时间";
                worksheet.Cells["M1"].Value = "发生时间";
                worksheet.Cells["N1"].Value = "交易对象";

                foreach (var item in db.Log.Where(i => i.UserId == client.UserId).OrderByDescending(i => i.HappenedAt))
                {
                    worksheet.Cells["A1"].Value = item.Id;
                    worksheet.Cells["B1"].Value = item.Type;
                    worksheet.Cells["C1"].Value = item.Money;
                    worksheet.Cells["D1"].Value = item.FrozenMoney;
                    worksheet.Cells["E1"].Value = item.AccountId;
                    worksheet.Cells["F1"].Value = item.ChannelId;
                    worksheet.Cells["G1"].Value = item.ProjectId;
                    worksheet.Cells["H1"].Value = item.BudgetId;
                    worksheet.Cells["I1"].Value = item.Remark;
                    worksheet.Cells["J1"].Value = item.OutTradeNo;
                    worksheet.Cells["K1"].Value = item.CreatedAt;
                    worksheet.Cells["L1"].Value = item.UpdatedAt;
                    worksheet.Cells["M1"].Value = item.HappenedAt;
                    worksheet.Cells["N1"].Value = item.TradingObject;
                }


                // 自动调整列宽
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                package.SaveAs(output);
            }
        }

        public static IDictionary<int, decimal> GetMonthLogs(IEnumerable<LogEntity> items, int maxDay)
        {
            var data = new Dictionary<int, decimal>();
            foreach (var item in items)
            {
                var day = item.HappenedAt.Day;
                if (!data.TryAdd(day, item.Money))
                {
                    data[day] += item.Money;
                }
            }
            var res = new OrderedDictionary<int, decimal>();
            for (int i = 1; i <= maxDay; i++)
            {
                if (!data.TryGetValue(i, out var val))
                {
                    val = 0;
                }
                res.Add(i, val);
            }
            return res;
        }
    }
}
