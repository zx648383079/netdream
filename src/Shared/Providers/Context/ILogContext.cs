using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Context
{
    public interface ILogContext
    {

        public DbSet<LogEntity> Logs { get; set; }

        public int SaveChanges();
    }
}
