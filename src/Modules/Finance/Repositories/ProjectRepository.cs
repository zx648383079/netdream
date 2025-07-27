using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Finance.Entities;
using NetDream.Modules.Finance.Forms;
using NetDream.Modules.Finance.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Finance.Repositories
{
    public class ProjectRepository(FinanceContext db, IClientContext client)
    {

        public ProjectListItem[] All()
        {
            var res = db.Project.Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.Id)
                .Select(i => new ProjectListItem() 
                {
                    Id = i.Id,
                    Name = i.Name,
                    Money = i.Money,
                    Earnings = i.Earnings,
                    StartAt = i.StartAt,
                    EndAt = i.EndAt,
                    Status = i.Status,
                    ProductId  = i.ProductId,
                    CreatedAt = i.CreatedAt,
                })
                .ToArray();
            ProductRepository.WithProduct(db, res);
            return res;
        }

        /**
         * 获取
         * @param int id
         * @return FinancialProjectModel
         * @throws Exception
         */
        public IOperationResult<FinancialProjectEntity> Get(int id)
        {
            var model = db.Project.Where(i => i.UserId == client.UserId && i.Id == id)
                .FirstOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<FinancialProjectEntity>("项目不存在");
            }
            return OperationResult.Ok(model);
        }

        /**
         * 保存
         * @param array data
         * @return FinancialProjectModel
         * @throws Exception
         */
        public IOperationResult<FinancialProjectEntity> Save(ProjectForm data)
        {
            FinancialProjectEntity? model;
            if (data.Id > 0)
            {
                model = db.Project.Where(i => i.UserId == client.UserId && i.Id == data.Id)
                .FirstOrDefault();
            }
            else
            {
                model = new();
            }
            if (model is null)
            {
                return OperationResult.Fail<FinancialProjectEntity>("项目不存在");
            }
            model.Name = data.Name;
            model.Money = data.Money;
            model.StartAt = data.StartAt;
            model.EndAt = data.EndAt;
            model.Earnings = data.Earnings;
            model.AccountId = data.AccountId;
            model.ProductId = data.ProductId;
            model.Alias = data.Alias;
            model.EarningsNumber = data.EarningsNumber;
            model.UserId = client.UserId;
            model.Remark = data.Remark;
            db.Project.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /**
         * 删除产品
         * @param int id
         * @return mixed
         */
        public void Remove(int id)
        {
            db.Project.Where(i => i.UserId == client.UserId && i.Id == id)
                .ExecuteDelete();
            db.SaveChanges();
        }

        /**
         * 项目收益
         * @param int id
         * @param float money
         * @return LogModel
         * @throws Exception
         */
        public IOperationResult<LogEntity> Earnings(int id, float money)
        {
            var project = db.Project.Where(i => i.UserId == client.UserId && i.Id == id)
                .FirstOrDefault();
            if (project is null)
            {
                return OperationResult.Fail<LogEntity>("项目不存在");
            }
            var model = new LogEntity
            {
                Money = (decimal)money,
                AccountId = project.AccountId,
                ProjectId = project.Id,
                Type = LogRepository.TYPE_INCOME,
                UserId = client.UserId,
                HappenedAt = DateTime.Now,
                Remark = $"理财项目 {project.Name} 收益"
            };
            db.Log.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }
    }
}
