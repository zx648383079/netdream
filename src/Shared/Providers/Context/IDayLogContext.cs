using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Context
{
    public interface IDayLogContext
    {

        public DbSet<DayLogEntity> DayLogs { get; set; }
    }
}
