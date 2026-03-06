using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Team.Entities;

namespace NetDream.Modules.Team.Migrations
{
    public class TeamEntityTypeConfiguration : IEntityTypeConfiguration<TeamEntity>
    {
        public void Configure(EntityTypeBuilder<TeamEntity> builder)
        {
            builder.ToTable("team", table => table.HasComment("团队系统"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0);
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(50).HasComment("群名");
            builder.Property(table => table.Logo).HasColumnName("logo").HasMaxLength(100).HasComment("群LOGO");
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty)
                .HasComment("群说明");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasComment("用户");
            builder.Property(table => table.OpenType).HasColumnName("open_type").HasDefaultValue(0).HasComment("群公开状态");
            builder.Property(table => table.OpenRule).HasColumnName("open_rule").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("类型匹配的值");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("审核状态");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
