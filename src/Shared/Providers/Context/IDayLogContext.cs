using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Context
{
    public interface IDayLogContext: ILogContext
    {

        public DbSet<DayLogEntity> DayLogs { get; set; }
    }
}
