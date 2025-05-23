using NetDream.Modules.Plan.Entities;
using NetDream.Modules.Plan.Models;
using NetDream.Shared.Interfaces;
using System;
using System.Linq;

namespace NetDream.Modules.Plan.Repositories
{
    internal class TaskRepository(PlanContext db, IClientContext client)
    {



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

        internal IOperationResult<TaskEntity> Stop(int id)
        {
            throw new NotImplementedException();
        }
    }
}