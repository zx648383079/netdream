using NetDream.Modules.Plan.Entities;
using NetDream.Modules.Plan.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Plan.Repositories
{
    public class DayRepository(PlanContext db, IClientContext client)
    {
        public const byte STATUS_NONE = 5;
        public const byte STATUS_RUNNING = 9;
        public const byte STATUS_PAUSE = 8;
        public IPage<DayListItem> GetList(string day, QueryForm form)
        {
            var res = db.Days.Where(i => i.UserId == client.UserId && i.Amount > 0
            && i.Today == day)
                .OrderByDescending(i => i.Status)
                .OrderBy(i => i.Id).ToPage<DayEntity, DayListItem>(form);
            TaskRepository.Include(db, res.Items);
            return res;
        }

        public IOperationResult<DayModel> Detail(int id)
        {
            var day = db.Days.Where(i => i.Id == id && i.UserId == client.UserId)
                .SingleOrDefault();
            if (day is null)
            {
                return OperationResult.Fail<DayModel>("任务不存在");
            }
            var res = day.CopyTo<DayModel>();
            var task = db.Tasks.Where(i => i.Id == day.TaskId).SingleOrDefault()
                .CopyTo<TaskModel>();
            task.Children = db.Tasks.Where(i => i.ParentId == day.TaskId).ToArray();
            res.Task = task;
            return OperationResult.Ok(res);
        }

        public IOperationResult<DayEntity> Save(int task_id, int id = 0, byte amount = 1)
        {
            if (id > 0)
            {
                var day = db.Days.Where(i => i.Id == id && i.UserId == client.UserId)
                .SingleOrDefault();
                if (day is null)
                {
                    return OperationResult<DayEntity>.Fail("错误");
                }
                day.Amount = amount;
                db.Days.Save(day, client.Now);
                db.SaveChanges();
                return OperationResult.Ok(day);
            }
            var task = db.Tasks.Where(i => i.Id == task_id && i.UserId == client.UserId).SingleOrDefault();
            if (task is null)
            {
                return OperationResult<DayEntity>.Fail("任务不存在");
            }
            return Add(task, amount);
        }

        public IOperationResult<DayEntity> Add(TaskEntity task,
            byte amount = 1)
        {
            var day = DateTime.Now.ToString("yyyy-MM-dd");
            var model = db.Days.Where(i => i.TaskId == task.Id && i.Today == day)
                .FirstOrDefault();
            if (model is not null)
            {
                model.Amount += amount;
                db.Days.Save(model, client.Now);
                db.SaveChanges();
                return OperationResult.Ok(model);
            }
            model = new DayEntity()
            {
                UserId = task.UserId,
                TaskId = task.Id,
                Today = day,
                Amount = amount,
                Status = STATUS_NONE
            };
            db.Days.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult Remove(int id)
        {
            var day = db.Days.Where(i => i.Id == id && i.UserId == client.UserId)
                .SingleOrDefault();
            if (day is null)
            {
                return OperationResult.Fail("任务不存在");
            }
            new TaskRepository(db, client).Stop(id);
            db.Days.Remove(day);
            db.SaveChanges();
            return OperationResult.Ok();
        }
    }
}
