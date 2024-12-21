using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Migrations
{
    public class MailLogEntityTypeConfiguration : IEntityTypeConfiguration<MailLogEntity>
    {
        public void Configure(EntityTypeBuilder<MailLogEntity> builder)
        {
            builder.ToTable("MailLog", table => table.HasComment(""));
            builder.HasKey("id");
        }
    }
}
