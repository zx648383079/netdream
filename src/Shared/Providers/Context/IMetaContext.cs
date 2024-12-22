using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Context
{
    public interface IMetaContext
    {

        public DbSet<MetaEntity> Metas { get; set; }

        public int SaveChanges();
    }
}
