using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Finance.Entities;
using NetDream.Modules.Finance.Models;
using NetDream.Modules.Finance.Repositories;
using System.Linq;

namespace NetDream.Modules.Finance
{
    public static class Extension
    {
        public static void ProvideFinanceRepositories(this IServiceCollection service)
        {
            service.AddScoped<ProductRepository>();
            service.AddScoped<AccountRepository>();
            service.AddScoped<BudgetRepository>();
            service.AddScoped<ChannelRepository>();
            service.AddScoped<LogRepository>();
            service.AddScoped<ProjectRepository>();
            service.AddScoped<StatisticsRepository>();
        }

        internal static IQueryable<LogListItem> SelectAs(this IQueryable<LogEntity> query)
        {
            return query.Select(i => new LogListItem()
            {
                Id = i.Id,
                ParentId = i.ParentId,
                Type = i.Type,
                Money = i.Money,
                FrozenMoney = i.FrozenMoney,
                ProjectId = i.ProjectId,
                BudgetId = i.BudgetId,
                AccountId = i.AccountId,
                ChannelId = i.ChannelId,
                Remark = i.Remark,
                HappenedAt = i.HappenedAt,
            });
        }
    }
}
