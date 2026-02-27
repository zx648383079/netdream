using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Plan.Entities;
using NetDream.Modules.Plan.Forms;
using NetDream.Modules.Plan.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Plan.Repositories
{
    public class TaskRepository(PlanContext db, IClientContext client)
    {
        public const byte STATUS_NONE = 5;
        public const byte STATUS_RUNNING = 9;
        public const byte STATUS_PAUSE = 8;

        public const byte STATUS_COMPLETE = 1;
        public const byte LOG_STATUS_NONE = 0;
        public const byte LOG_STATUS_PAUSE = 1; // 暂停
        public const byte LOG_STATUS_FINISH = 2; // 完成
        public const byte LOG_STATUS_FAILURE = 3; // 中断失败，未完成一个番茄时间
        public IPage<TaskListItem> GetList(TaskQueryForm form)
        {
            return db.Tasks.Where(i => i.UserId == client.UserId && i.ParentId == form.ParentId)
                .Search(form.Keywords, "name")
                .When(form.Status == 1, i => i.Status >= 5)
                .When(form.Status > 1, i => i.Status == STATUS_COMPLETE)
                .OrderByDescending(i => i.Status)
                .ThenByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
        }

        public IOperationResult<TaskModel> Detail(int id)
        {
            var model = db.Tasks.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<TaskModel>.Fail("任务不存在");
            }
            var res = model.CopyTo<TaskModel>();
            res.Children = db.Tasks.Where(i => i.ParentId == id).ToArray();
            return OperationResult.Ok(res);
        }

        public IOperationResult<TaskEntity> Save(
            TaskForm data, int status = -1)
        {
            var model = data.Id > 0 ? db.Tasks.Where(i => i.Id == data.Id && i.UserId == client.UserId).SingleOrDefault() 
                : new TaskEntity();
            if (model is null)
            {
                return OperationResult<TaskEntity>.Fail("任务不存在");
            }
            model.StartAt = data.StartAt;
            model.Description = data.Description;
            model.Name = data.Name;
            model.PerTime = data.PerTime;
            model.EveryTime = data.EveryTime;
            model.ParentId = data.ParentId;
            model.SpaceTime = data.SpaceTime;

            model.UserId = client.UserId;
            if (status >= 0 && model.Status == STATUS_COMPLETE
                && status != STATUS_COMPLETE)
            {
                model.Status = 0;
            }
            db.Tasks.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult Remove(int id, bool stop = false)
        {
            var model = db.Tasks.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("任务不存在");
            }
            StopTask(id);
            if (!stop)
            {
                db.Tasks.Remove(model);
                db.SaveChanges();
            }
            return OperationResult.Ok();
        }

        public TaskEntity[] GetActiveTask()
        {
            return db.Tasks.Where(i => i.UserId == client.UserId && i.Status == STATUS_NONE)
                .OrderByDescending(i => i.Status)
                .OrderBy(i => i.Id).ToArray();
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="id"></param>
        /// <param name="child_id"></param>
        /// <returns></returns>
        public IOperationResult<DayEntity> Start(int id, int child_id = 0)
        {
            var other = db.Days.Where(i => 
                i.UserId == client.UserId && i.Id != id && i.Status > STATUS_NONE)
                .Select(i => i.Id).ToArray();
            foreach (var item in other)
            {
                Stop(item);
            }
            var day = db.Days.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (day is null)
            {
                return OperationResult<DayEntity>.Fail("任务不存在");
            }
            if (day.Status == STATUS_RUNNING)
            {
                return OperationResult.Ok(day);
            }
            var task = db.Tasks.Where(i => i.Id == day.TaskId).Single();
            LogEntity? log = null;
            if (day.Status == STATUS_PAUSE)
            {
                var log_list = db.Logs.Where(i => i.TaskId == day.TaskId
                    && (i.Status == LOG_STATUS_NONE || i.Status == LOG_STATUS_PAUSE))
                    .OrderByDescending(i => i.Id)
                    .ToArray();
                foreach (var item in log_list)
                {
                    if (item.DayId == day.Id)
                    {
                        log = item;
                        continue;
                    }
                    StopDayLog(item);
                }
                if (log is not null)
                {
                    log.OutageTime += client.Now - log.EndAt;
                    log.EndAt = 0;
                    log.Status = LOG_STATUS_NONE;
                    db.Logs.Save(log, client.Now);
                    day.Status = STATUS_RUNNING;
                    db.Days.Save(day, client.Now);
                    task.Status = STATUS_RUNNING;
                    db.Tasks.Save(task, client.Now);
                    if (log.ChildId > 0)
                    {
                        db.Tasks.Where(i => i.Id == log.ChildId)
                            .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, STATUS_RUNNING));
                    }
                    db.SaveChanges();
                    return OperationResult.Ok(day);
                }
            }
            if (day.Amount < 1)
            {
                return OperationResult<DayEntity>.Fail("今日任务已完成");
            }
            log = new LogEntity()
            {
                UserId = client.UserId,
                TaskId = day.TaskId,
                ChildId = child_id,
                DayId = day.Id,
            };
            db.Logs.Save(log, client.Now);
            day.Status = STATUS_RUNNING;
            db.Days.Save(day, client.Now);
            task.Status = STATUS_RUNNING;
            db.Tasks.Save(task, client.Now);
            if (log.ChildId > 0)
            {
                db.Tasks.Where(i => i.Id == log.ChildId)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, STATUS_RUNNING));
            }
            db.SaveChanges();
            return OperationResult.Ok(day);
        }

        private IOperationResult StopDayLog(LogEntity log)
        {
            var day = db.Days.Where(i => i.Id == log.DayId 
                && i.UserId == client.UserId).SingleOrDefault();
            if (log.Status != LOG_STATUS_PAUSE)
            {
                log.EndAt = client.Now;
            }
            var time = GetSpentTime(log);
            if (day is null)
            {
                log.Status = LOG_STATUS_FAILURE;
                db.Logs.Save(log, client.Now);
                db.SaveChanges();
                return OperationResult.Ok();
            }
            var task = db.Tasks.Where(i => i.Id == day.TaskId).Single();
            log.Status =
                task.EveryTime <= 0 ||
                time >= task.EveryTime * 60
                    ? LOG_STATUS_FINISH : LOG_STATUS_FAILURE;
            db.Logs.Save(log, client.Now);
            if (day.Status == STATUS_NONE)
            {
                db.SaveChanges();
                return OperationResult.Ok();
            }
            AddDayTime(task, day, log, time);
            db.SaveChanges();
            return OperationResult.Ok();
        }

        /// <summary>
        /// 需要手动调用 db.SaveChanges();
        /// </summary>
        /// <param name="task"></param>
        /// <param name="day"></param>
        /// <param name="log"></param>
        /// <param name="time"></param>
        private void AddDayTime(TaskEntity task, DayEntity day, LogEntity log, int time)
        {
            task.TimeLength += time;
            task.Status = STATUS_NONE;
            db.Tasks.Save(task, client.Now);
            if (log.ChildId > 0)
            {
                db.Tasks.Where(i => i.Id == log.ChildId)
                   .ExecuteUpdate(setters => setters.SetProperty(i => i.TimeLength, i => i.TimeLength + time)
                   .SetProperty(i => i.Status, STATUS_NONE));
            }
            if (log.Status == LOG_STATUS_FINISH)
            {
                day.SuccessAmount++;
                if (day.Amount > 0)
                {
                    day.Amount--;
                }
            }
            else
            {
                day.FailureAmount++;
            }
            day.Status = STATUS_NONE;
            db.Days.Save(day, client.Now);
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IOperationResult<DayEntity> Stop(int id)
        {
            var day = db.Days.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (day is null)
            {
                return OperationResult<DayEntity>.Fail("任务不存在");
            }
            if (day.Status == STATUS_NONE)
            {
                return OperationResult.Ok(day);
            }
            var log = GetRunningLog(day.TaskId);
            if (log is null)
            {
                return OperationResult<DayEntity>.Fail("停止失败");
            }
            if (log.Status != LOG_STATUS_PAUSE)
            {
                log.EndAt = client.Now;
            }
            var time = GetSpentTime(log);
            var task = db.Tasks.Where(i => i.Id == day.TaskId).Single();
            log.Status =
                task.EveryTime <= 0 ||
                time >= task.EveryTime * 60
                    ? LOG_STATUS_FINISH : LOG_STATUS_FAILURE;
            db.Logs.Save(log, client.Now);
            AddDayTime(task, day, log, time);
            db.SaveChanges();
            return OperationResult.Ok(day);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IOperationResult<DayEntity> Pause(int id)
        {
            var day = db.Days.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (day is null)
            {
                return OperationResult<DayEntity>.Fail("任务不存在");
            }
            if (day.Status == STATUS_PAUSE)
            {
                return OperationResult.Ok(day);
            }
            var log = GetRunningLog(day.TaskId);
            if (log is null)
            {
                return OperationResult<DayEntity>.Fail("暂停失败");
            }
            log.EndAt = client.Now;
            log.Status = LOG_STATUS_PAUSE;
            db.Logs.Save(log, client.Now);
            day.PauseAmount++;
            day.Status = STATUS_PAUSE;
            db.Days.Save(day, client.Now);
            var task = db.Tasks.Where(i => i.Id == day.TaskId).Single();
            task.Status = STATUS_PAUSE;
            db.Tasks.Save(task, client.Now);
            if (log.ChildId > 0)
            {
                db.Tasks.Where(i => i.Id == log.ChildId)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, STATUS_PAUSE));
            }
            return OperationResult.Ok(day);
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IOperationResult<DayEntity?> Check(int id)
        {
            var day = db.Days.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (day is null)
            {
                return OperationResult<DayEntity>.Fail("任务不存在");
            }
            var task = db.Tasks.Where(i => i.Id == day.TaskId).Single();
            if (day.Status != STATUS_RUNNING
                || task.EveryTime <= 0)
            {
                return OperationResult.Ok<DayEntity>(null);
            }
            var log = GetRunningLog(task.Id);
            if (log is null)
            {
                return OperationResult<DayEntity>.Fail("任务记录有问题");
            }
            log.EndAt = client.Now;
            var time = GetSpentTime(log);
            if (time < task.EveryTime * 60)
            {
                return OperationResult.Ok<DayEntity>(null);
            }
            log.Status = LOG_STATUS_FINISH;
            db.Logs.Save(log, client.Now);
            task.TimeLength += time;
            task.Status = STATUS_NONE;
            db.Tasks.Save(task, client.Now);
            day.SuccessAmount++;
            day.Amount--;
            day.Status = STATUS_NONE;
            db.Days.Save(day, client.Now);
            if (log.ChildId > 0)
            {
                db.Tasks.Where(i => i.Id == log.ChildId)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.TimeLength, i => i.TimeLength + time)
                    .SetProperty(i => i.Status, STATUS_NONE));
            }
            db.SaveChanges();
            return OperationResult.Ok(day);
        }

        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IOperationResult<TaskEntity> StopTask(int id)
        {
            var task = db.Tasks.Where(i => i.Id == id && i.UserId == client.UserId).SingleOrDefault();
            if (task is null || task.Status == STATUS_COMPLETE)
            {
                return OperationResult<TaskEntity>.Fail("任务不存在");
            }
            if (task.Status != STATUS_NONE)
            {
                StopLog(task);
                CancelDay(task);
            }
            task.Status = STATUS_COMPLETE;
            db.Tasks.Save(task, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(task);
        }

        private void StopLog(TaskEntity task)
        {
            var log = GetRunningLog(task.Id);
            if (log is null)
            {
                return;
            }
            if (log.Status != LOG_STATUS_PAUSE)
            {
                log.EndAt = client.Now;
            }
            var time = GetSpentTime(log);
            log.Status =
                task.EveryTime <= 0 || time >= task.EveryTime * 60
                    ? LOG_STATUS_FINISH : LOG_STATUS_FAILURE;
            db.Logs.Save(log, client.Now);
            task.TimeLength += time;
            task.Status = STATUS_NONE;
            db.Tasks.Save(task, client.Now);
            if (task.ParentId > 0)
            {
                db.Tasks.Where(i => i.Id == task.ParentId)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.TimeLength, i => i.TimeLength + time)
                    .SetProperty(i => i.Status, STATUS_NONE));
            }
            db.SaveChanges();
            if (log.DayId < 1)
            {
                return;
            }
            var day = db.Days.Where(i => i.Id == log.DayId).SingleOrDefault();
            if (day is null)
            {
                return;
            }
            if (log.Status == LOG_STATUS_FINISH)
            {
                day.SuccessAmount++;
            }
            else
            {
                day.FailureAmount++;
            }
            day.Amount = 0;
            day.Status = STATUS_NONE;
            db.Days.Save(day, client.Now);
            db.SaveChanges();
        }

        private void CancelDay(TaskEntity task)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            db.Days.Where(i => i.TaskId == task.Id && i.Today > today)
                .ExecuteDelete();
            db.SaveChanges();
        }

        public string RestTip(DayEntity model)
        {
            // 记录今天完成的任务次数，每4轮多休息
            var today = TimeHelper.TimestampFrom(DateTime.Today);
            var count = db.Logs.Where(i => i.CreatedAt > today 
                && i.Status == LOG_STATUS_FINISH
                && i.UserId == client.UserId).Count();
            if (count % 4 == 0)
            {
                return "本轮任务完成，请休息20-25分钟";
            }
            return "本轮任务完成，请休息3-5分钟";
        }

        internal static void Include(PlanContext db, IWithTaskModel[] items)
        {
            var idItems = items.Select(item => item.TaskId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Tasks.Where(i => idItems.Contains(i.Id))
                .Select(i => new TaskLabelItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                })
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.TaskId > 0 && data.TryGetValue(item.TaskId, out var res))
                {
                    item.Task = res;
                }
            }
        }

        public LogEntity? GetRunningLog(int taskId)
        {
            return db.Logs.Where(i => (i.TaskId == taskId || i.ChildId == taskId) && (
                i.Status == LOG_STATUS_NONE || i.Status == LOG_STATUS_PAUSE))
                .OrderByDescending(i => i.Status).FirstOrDefault();
        }

        public int GetSpentTime(LogEntity log)
        {
            return GetSpentTime(log, client.Now);
        }

        public static int GetSpentTime(LogEntity log, int now)
        {
            var endAt = log.EndAt;
            if (log.EndAt == 0)
            {
                endAt = now;
            }
            return endAt - log.CreatedAt - log.OutageTime;
        }
    }
}