using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Counter.Entities;

namespace NetDream.Modules.Counter.Migrations
{
    public class HostnameEntityTypeConfiguration : IEntityTypeConfiguration<HostnameEntity>
    {
        public void Configure(EntityTypeBuilder<HostnameEntity> builder)
        {
            builder.ToTable("ctr_hostname", table => table.HasComment("ÓòÃû"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name");
        }
    }
}
