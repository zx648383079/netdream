using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Plan.Entities;
using NetDream.Modules.Plan.Models;
using NetDream.Modules.Plan.Repositories;
using System.Linq;

namespace NetDream.Modules.Plan
{
    public static class Extension
    {
        public static void ProvidePlanRepositories(this IServiceCollection service)
        {
            service.AddScoped<PlanRepository>();
            service.AddScoped<CommentRepository>();
            service.AddScoped<DayRepository>();
            service.AddScoped<ReviewRepository>();
            service.AddScoped<ShareRepository>();
            service.AddScoped<TaskRepository>();
        }

        public static IQueryable<TaskListItem> SelectAs(this IQueryable<TaskEntity> query)
        {
            return query.Select(i => new TaskListItem()
            {
                Id = i.Id,
                Name = i.Name,
                ParentId = i.ParentId,
                Description = i.Description,
                SpaceTime = i.SpaceTime,
                StartAt = i.StartAt,
                EveryTime = i.EveryTime,
                Status = i.Status,
                CreatedAt = i.CreatedAt,
                PerTime = i.PerTime,
                TimeLength = i.TimeLength,
            });
        }
    }
}
