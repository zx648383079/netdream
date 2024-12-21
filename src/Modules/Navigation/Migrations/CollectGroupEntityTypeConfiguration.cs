using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Navigation.Entities;

namespace NetDream.Modules.Navigation.Migrations
{
    public class CollectGroupEntityTypeConfiguration : IEntityTypeConfiguration<CollectGroupEntity>
    {
        public void Configure(EntityTypeBuilder<CollectGroupEntity> builder)
        {
            builder.ToTable("search_collect_group", table => table.HasComment("收藏分组表"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(1).HasDefaultValue(5);
        }
    }
}
