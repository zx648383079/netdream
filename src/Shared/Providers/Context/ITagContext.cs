using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Context
{
    public interface ITagContext
    {

        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<TagLinkEntity> TagLinks { get; set; }

        public int SaveChanges();
    }
}
