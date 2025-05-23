using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Plan.Entities;
using NetDream.Modules.Plan.Forms;
using NetDream.Modules.Plan.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Plan.Repositories
{
    public class PlanRepository(PlanContext db, IClientContext client)
    {
        public IPage<PlanListItem> GetList(QueryForm form, byte type)
        {
            var res = db.Plans.Where(i => i.PlanType == type && i.UserId == client.UserId)
                .OrderByDescending(i => i.Id).ToPage<PlanEntity, PlanListItem>(form);
            TaskRepository.Include(db, res.Items);
            return res;
        }


        public IOperationResult<PlanEntity> Save(PlanForm data)
        {
            var model = data.Id > 0 ? db.Plans.Where(i => i.Id == data.Id 
            && i.UserId == client.UserId).SingleOrDefault() : new PlanEntity();
            if (model == null)
            {
                return OperationResult<PlanEntity>.Fail("任务错误");
            }
            if (data.Id == 0)
            {
                model.TaskId = data.TaskId;
            }
            var task = db.Tasks.Where(i => i.Id == model.TaskId && i.UserId == client.UserId).SingleOrDefault();
            if (task is null)
            {
                return OperationResult<PlanEntity>.Fail("任务错误");
            }
            model.PlanType = data.PlanType;
            model.Amount = data.Amount;
            if (model.PlanType < 1)
            {
                model.PlanTime = Validator.IsNumeric(data.PlanTime) ? int.Parse(data.PlanTime) : TimeHelper.TimestampFrom(data.PlanTime);
            }
            else
            {
                model.PlanTime = Math.Max(int.Parse(data.PlanTime), 1);
            }
            model.UserId = client.UserId;
            var other = db.Plans.Where(i => i.Id != data.Id && i.UserId == client.UserId
            && i.TaskId == model.TaskId && i.PlanType == model.PlanType).FirstOrDefault();
            if (other is not null)
            {
                other.Amount += model.Amount;
                other.PlanTime = model.PlanTime;
                model = other;
            }
            db.Plans.Save(model, client.Now);
            db.SaveChanges();
            if (data.Id > 0 && model.Id != data.Id)
            {
                Remove(data.Id);
            }
            var res = model.CopyTo<PlanListItem>();
            res.Task = task.CopyTo<TaskLabelItem>();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Plans.Where(i => i.Id == id && i.UserId == client.UserId)
                .ExecuteDelete();
            db.SaveChanges();
        }
    }
}
