using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Team.Entities;
using NetDream.Modules.Team.Models;
using NetDream.Modules.Team.Repositories;
using NetDream.Shared.Interfaces;
using System.Linq;

namespace NetDream.Modules.Team
{
    public static class Extension
    {
        public static void ProvideTeamRepositories(this IServiceCollection service)
        {
            service.AddScoped<TeamRepository>();
            service.AddScoped<ITeamRepository, TeamRepository>();
        }

        internal static IQueryable<TeamLabelItem> SelectAsLabel(this IQueryable<TeamEntity> query)
        {
            return query.Select(i => new TeamLabelItem()
            {
                Id = i.Id,
                Name = i.Name,
                Logo = i.Logo,
            });
        }

        internal static IQueryable<TeamListItem> SelectAs(this IQueryable<TeamEntity> query)
        {
            return query.Select(i => new TeamListItem()
            {
                Id = i.Id,
                Name = i.Name,
                Logo = i.Logo,
                Description = i.Description,
                UserId = i.UserId,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
            });
        }

        internal static IQueryable<TeamUserListItem> SelectAs(this IQueryable<TeamUserEntity> query)
        {
            return query.Select(i => new TeamUserListItem()
            {
                Id = i.UserId,
                TeamId = i.TeamId,
                Name = i.Name,
                RoleId = i.RoleId,
            });
        }
    }
}
